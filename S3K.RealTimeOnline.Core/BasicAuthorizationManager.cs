using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using S3K.RealTimeOnline.GenericDataAccess.Tools;
using S3K.RealTimeOnline.SecurityDataAccess.QueryHandlers.FindUserByUsernamePassword;
using S3K.RealTimeOnline.SecurityDomain;
using Unity;

namespace S3K.RealTimeOnline.Core
{
    public class BasicAuthorizationManager : ServiceAuthorizationManager
    {
        private IUnityContainer _container;

        public BasicAuthorizationManager(IUnityContainer container)
        {
            _container = container;
        }

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            IPrincipal principal = null;

            if (ContextHelper.GetRoleName(operationContext) != null)
            {
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
                        FindUserByUsernamePasswordQuery query =
                            new FindUserByUsernamePasswordQuery
                            {
                                Username = credentials[0],
                                Password = credentials[1]
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
                }
            }
            else
            {
                principal = new CustomPrincipal(new GenericIdentity("Anonymous"), new string[] { });
            }

            if (principal != null)
            {
                Thread.CurrentPrincipal = principal;
                operationContext.IncomingMessageProperties.Add("Principal", principal);
                operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] =
                    principal;
                return true;
            }


            UriTemplateMatch uriTemplateMatchResults = (UriTemplateMatch)
                operationContext.IncomingMessageProperties["UriTemplateMatchResults"];
            var webContext = new WebOperationContext(operationContext);
            webContext.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
            webContext.OutgoingResponse.Headers.Add(HttpResponseHeader.WwwAuthenticate,
                string.Format("Basic realm=\"{0}\"", uriTemplateMatchResults.BaseUri.AbsoluteUri));
            //throw new WebFaultException(HttpStatusCode.Unauthorized);
            return false;
        }
    }
}