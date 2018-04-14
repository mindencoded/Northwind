using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace S3K.RealTimeOnline.Core
{
    public class AnonymousAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string role = ContextHelper.GetRoleName(operationContext); // ?? "AllowAnonymous";
            IPrincipal principal = new CustomPrincipal(new GenericIdentity("Anonymous"), new[] {role});
            Thread.CurrentPrincipal = principal;
            operationContext.IncomingMessageProperties.Add("Principal", principal);
            operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] = principal;
            return true;
        }
    }
}