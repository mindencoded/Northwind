using System;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using Unity;

namespace S3K.RealTimeOnline.Core.Security
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
            HttpRequestMessageProperty httpRequest =
                operationContext.IncomingMessageProperties["httpRequest"] as HttpRequestMessageProperty;
            if (httpRequest != null)
            {
                string authorizationHeader = httpRequest.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    string[] credentials = Encoding.UTF8
                        .GetString(Convert.FromBase64String(authorizationHeader.Substring(6)))
                        .Split(':');

                    if (credentials.Length == 2 && credentials[0] == "testuser" &&
                        credentials[1] == "testpassword")
                    {
                        string[] roles =
                        {
                            "CustomerCrud.Select",
                            "RoleGroupCrud.Select"
                        };
                        IPrincipal principal = new CustomPrincipal(new GenericIdentity("testuser"), roles);
                        Thread.CurrentPrincipal = principal;
                        operationContext.IncomingMessageProperties.Add("Principal", principal);
                        operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] =
                            principal;
                        return true;
                    }
                }
                UriTemplateMatch uriTemplateMatchResults =
                    operationContext.IncomingMessageProperties["UriTemplateMatchResults"] as UriTemplateMatch;
                if (uriTemplateMatchResults != null)
                {
                    var webContext = new WebOperationContext(operationContext);
                    webContext.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                    webContext.OutgoingResponse.Headers.Add(HttpResponseHeader.WwwAuthenticate,
                        string.Format("Basic realm=\"{0}\"", uriTemplateMatchResults.BaseUri.AbsoluteUri));
                }
            }
            throw new WebFaultException<InfoMessage>(new InfoMessage("Unauthorized"), HttpStatusCode.Unauthorized);
        }
    }
}