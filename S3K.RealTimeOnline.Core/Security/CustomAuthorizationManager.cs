using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDataAccess.QueryHandlers.FindUserByUsernamePassword;
using S3K.RealTimeOnline.SecurityDomain;
using Unity;

namespace S3K.RealTimeOnline.Core.Security
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        private IUnityContainer _container;

        public CustomAuthorizationManager(IUnityContainer container)
        {
            _container = container;
        }

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            object value = operationContext.IncomingMessageProperties.TryGetValue("Principal", out value)
                ? value
                : null;
            IPrincipal principal = value as IPrincipal;
            if (principal != null) return true;
            if (ContextHelper.GetRoleName(operationContext) != null)
            {
                UriTemplateMatch uriTemplateMatch =
                    (UriTemplateMatch) operationContext.IncomingMessageProperties["UriTemplateMatchResults"];
                HttpRequestMessageProperty httpRequest = (HttpRequestMessageProperty)
                    operationContext.IncomingMessageProperties["httpRequest"];
                string authorizationHeader = httpRequest.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    string[] credentials = Encoding.UTF8
                        .GetString(Convert.FromBase64String(authorizationHeader.Substring(6)))
                        .Split(':');

                    if (credentials.Length == 2)
                    {
                        string username = credentials[0];
                        string password = Md5Hash.Create(credentials[1]);
                        FindUserByUsernamePasswordQuery query =
                            new FindUserByUsernamePasswordQuery
                            {
                                Username = username,
                                Password = password
                            };
                        IQueryHandler<FindUserByUsernamePasswordQuery, User> handler =
                            _container.Resolve<IQueryHandler<FindUserByUsernamePasswordQuery, User>>();
                        User user = handler.Handle(query);
                        if (user != null)
                        {
                            IList<string> roles = user.RoleDetails.Select(x => x.Role.Name).ToList();
                            principal = new CustomPrincipal(new GenericIdentity(user.Username), roles.ToArray());
                        }
                    }

                    WebOperationContext webContext = new WebOperationContext(operationContext);
                    webContext.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    webContext.OutgoingResponse.Headers.Add(HttpResponseHeader.WwwAuthenticate,
                        string.Format("Basic realm=\"{0}\"", uriTemplateMatch.BaseUri.AbsoluteUri));
                }
            }
            else
            {
                principal = new CustomPrincipal(new GenericIdentity("Anonymous"), new string[] { });
            }

            if (principal != null)
            {
                operationContext.IncomingMessageProperties.Add("Principal", principal);
            }

            return true;
        }
    }
}