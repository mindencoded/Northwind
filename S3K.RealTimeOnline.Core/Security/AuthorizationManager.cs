using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Unity;

namespace S3K.RealTimeOnline.Core.Security
{
    public class AuthorizationManager : ServiceAuthorizationManager
    {
        private IUnityContainer _container;

        public AuthorizationManager(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>  
        /// Method source sample taken from here: http://bit.ly/1hUa1LR  
        /// </summary>  
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            //Extract the Authorization header, and parse out the credentials converting the Base64 string:  
            if (WebOperationContext.Current != null)
            {
                IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                WebHeaderCollection headers = request.Headers;
                Uri requestUri = request.UriTemplateMatch.RequestUri;
                string absolutePath = requestUri.AbsolutePath;
                foreach (string headerName in headers.AllKeys)
                {
                    Trace.WriteLine(string.Format("{0} : {1}", headerName, headers[headerName]));
                }
                var authHeader = request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authHeader))
                {
                    var svcCredentials = Encoding.UTF8
                        .GetString(Convert.FromBase64String(authHeader.Substring(6)))
                        .Split(':');
                    dynamic user = new
                    {
                        Name = svcCredentials[0],
                        Password = svcCredentials[1]
                    };
                    if (user.Name == "testuser" && user.Password == "testpassword")
                    {
                        //User is authrized and originating call will proceed  
                        return true;
                    }
                }
                //No authorization header was provided, so challenge the client to provide before proceeding:  
                WebOperationContext.Current.OutgoingResponse.Headers.Add(
                    "WWW-Authenticate: Basic realm=\"" + absolutePath + "\"");
            }
            //Throw an exception with the associated HTTP status code equivalent to HTTP status 401  
            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}