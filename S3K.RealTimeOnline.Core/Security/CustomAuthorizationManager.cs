using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Policy;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
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
            HttpRequestMessageProperty httpRequest = operationContext.IncomingMessageProperties["httpRequest"] as HttpRequestMessageProperty;
            if (httpRequest != null)
            {
                string authorizationHeader = httpRequest.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    string[] svcCredentials = Encoding.UTF8
                        .GetString(Convert.FromBase64String(authorizationHeader.Substring(6)))
                        .Split(':');

                    if (svcCredentials.Length == 2 && svcCredentials[0] == "testuser" && svcCredentials[1] == "testpassword")
                    {
                        string[] roles =
                        {
                            "CustomerCrud.Select"
                        };

                        /* IList<Claim> claims = new List<Claim>();
                         claims.Add(new Claim(ClaimTypes.Name, "testuser"));
                         foreach (var role in roles)
                         {
                             claims.Add(new Claim(ClaimTypes.Role, role));
                         }

                         ClaimsIdentity identity = new ClaimsIdentity(claims);
                         ClaimsPrincipal principal = new ClaimsPrincipal(identity*/

                        GenericIdentity identity = new GenericIdentity("testuser");
                        GenericPrincipal principal = new GenericPrincipal(identity, roles);
                        //CustomPrincipal principal = new CustomPrincipal(identity, roles);

                        // set principal in thread and operation context
                        Thread.CurrentPrincipal = principal;
                        operationContext.IncomingMessageProperties.Add("Principal", principal);
                        OperationContext.Current = operationContext;

                        return true;
                    }
                }
            }

            UriTemplateMatch uriTemplateMatchResults =
                operationContext.IncomingMessageProperties["UriTemplateMatchResults"] as UriTemplateMatch;
            if (uriTemplateMatchResults != null)
            {
                operationContext.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader("WWW-Authenticate", uriTemplateMatchResults.BaseUri.Host,
                    "Basic realm=\"" + uriTemplateMatchResults.BaseUri + "\""));
            }


           /* if (WebOperationContext.Current != null)
            {
                IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                string authorizationHeader = request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    string[] svcCredentials = Encoding.UTF8
                        .GetString(Convert.FromBase64String(authorizationHeader.Substring(6)))
                        .Split(':');

                    if (svcCredentials.Length == 2 && svcCredentials[0] == "testuser" && svcCredentials[1] == "testpassword")
                    {
                        string[] roles =
                        {
                            "CustomerCrud.Select"
                        };

                        //IList<Claim> claims = new List<Claim>();
                        //claims.Add(new Claim(ClaimTypes.Name, "testuser"));
                        //foreach (var role in roles)
                        //{
                        //    claims.Add(new Claim(ClaimTypes.Role, role));
                        //}
                        //ClaimsIdentity identity = new ClaimsIdentity(claims);
                        //ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                        GenericIdentity identity = new GenericIdentity("testuser");
                        //GenericPrincipal principal = new GenericPrincipal(identity, roles);
                        CustomPrincipal principal = new CustomPrincipal(identity, roles);

                        // set principal in thread and operation context
                        Thread.CurrentPrincipal = principal;
                        operationContext.IncomingMessageProperties.Add("Principal", principal);
                        operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] = principal;
                        OperationContext.Current = operationContext;

                        return true;
                    }
                }
                //No authorization header was provided, so challenge the client to provide before proceeding
                WebOperationContext.Current.OutgoingResponse.Headers.Add(
                    "WWW-Authenticate: Basic realm=\"" + request.UriTemplateMatch.RequestUri.Host + "\"");
            }*/

                //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}