using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.Contracts.Services;
using S3K.RealTimeOnline.Core.Decorators;
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
                    string privateKey = RsaTokenTool.GetXmlString(AppConfig.PrivateKeyPath);
                    //  string token = new JwtTokenGenerator(privateKey, uriTemplateMatch.BaseUri.Host, uriTemplateMatch.BaseUri.Host, AppConfig.TokenExpirationMinutes).Encode(login.Username, null, roles);
                    JwtTokenTool jwtTokenTool = new JwtTokenTool(privateKey, uriTemplateMatch.BaseUri.Host, uriTemplateMatch.BaseUri.Host, AppConfig.TokenExpirationMinutes);
                    string token = jwtTokenTool.CreateRsaJwtSecurityToken(login.Username, null, roles);
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