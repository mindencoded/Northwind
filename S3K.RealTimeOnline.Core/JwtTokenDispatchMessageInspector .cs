using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace S3K.RealTimeOnline.Core
{
    public class JwtTokenDispatchMessageInspector : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            HttpRequestMessageProperty httpRequest = (HttpRequestMessageProperty)
                OperationContext.Current.IncomingMessageProperties["httpRequest"];
            string encryptedToken = httpRequest.Headers["JWTTOKEN"];
            if (!string.IsNullOrEmpty(encryptedToken))
            {
                ClaimsPrincipal claimsPrincipal;
                bool isValid = new JwtTokenValidator().Validate(encryptedToken, out claimsPrincipal);
                if (isValid)
                {
                    ClaimsIdentity claimsIdentity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
                    if (claimsIdentity != null)
                    {
                        foreach (var claim in claimsPrincipal.Claims)
                        {
                            claimsIdentity.AddClaim(new Claim(claim.Type, claim.Value));
                        }
                    }

                    Claim nameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                    if (nameClaim != null)
                    {
                        string[] roles = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value).ToArray();
                        IPrincipal principal = new CustomPrincipal(new GenericIdentity(nameClaim.Value), roles);
                        Thread.CurrentPrincipal = principal;
                        OperationContext.Current.IncomingMessageProperties.Add("Principal", principal);
                        OperationContext.Current.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] =
                            principal;
                    }
                }
            }

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            //throw new NotImplementedException();
        }
    }
}