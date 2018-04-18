using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Core.Decorators;
using S3K.RealTimeOnline.Core.Security;
using S3K.RealTimeOnline.Dtos;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDataAccess.QueryHandlers.SelectRolesByUserName;
using S3K.RealTimeOnline.SecurityDataAccess.QueryHandlers.VerifyUsernamePassword;
using S3K.RealTimeOnline.SecurityDomain;
using Unity;

namespace S3K.RealTimeOnline.Core.Services
{
    public class AccountService : RestService, IAccountService
    {
        public AccountService(IUnityContainer container) : base(container)
        {
        }

        public static void Configure(ServiceConfiguration config)
        {
            WebHttpConfigure<IAccountService>(config, "");
        }

        public Stream Login(LoginDto login)
        {
            try
            {
                VerifyUsernamePasswordQuery verifyUsernamePasswordQuery =
                    new VerifyUsernamePasswordQuery
                    {
                        Username = login.Username,
                        Password = login.Password
                    };

                IQueryHandler<VerifyUsernamePasswordQuery, bool> verifyUsernamePasswordQueryHandler =
                    Container.Resolve<IQueryHandler<VerifyUsernamePasswordQuery, bool>>(HandlerDecoratorType
                        .ValidationCommand.ToString());
                if (verifyUsernamePasswordQueryHandler.Handle(verifyUsernamePasswordQuery))
                {
                    SelectRolesByUserNameQuery selectRolesByUserNameQuery = new SelectRolesByUserNameQuery
                    {
                        Username = login.Username
                    };
                    IQueryHandler<SelectRolesByUserNameQuery, IEnumerable<Role>> selectRolesByUserNameQueryHandler =
                        Container.Resolve<IQueryHandler<SelectRolesByUserNameQuery, IEnumerable<Role>>>();
                    string[] roles = selectRolesByUserNameQueryHandler.Handle(selectRolesByUserNameQuery)
                        .Select(r => r.Name).ToArray();

                    UriTemplateMatch uriTemplateMatch =
                        (UriTemplateMatch) OperationContext.Current
                            .IncomingMessageProperties["UriTemplateMatchResults"];
                    string token;
                    string name = login.Username;
                    if (AppConfig.UseRsa)
                    {
                        RSACryptoServiceProvider rsa = RsaStore.Get("Custom");
                        token = new JwtRsaGenerator(uriTemplateMatch.BaseUri.Host,
                            uriTemplateMatch.BaseUri.Host,
                            AppConfig.TokenExpirationMinutes).Encode(rsa, name, null, roles);
                    }
                    else
                    {
                        byte[] symmetricKey = HmacStore.Get("Custom");
                        token = new JwtHmacGenerator(uriTemplateMatch.BaseUri.Host,
                                uriTemplateMatch.BaseUri.Host, AppConfig.TokenExpirationMinutes)
                            .Encode(symmetricKey, name, null, roles);
                    }
                    string data = DataToString(new {JWTTOKEN = token});
                    return CreateStreamResponse(data);
                }
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(ex), ex is ValidationException
                    ? HttpStatusCode.BadRequest
                    : HttpStatusCode.InternalServerError);
            }
        }
    }
}