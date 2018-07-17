using System.Security.Principal;
using System.ServiceModel;

namespace S3K.RealTimeOnline.WebService.Security
{
    public class AnonymousAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string role = ContextHelper.GetRoleName(operationContext);
            IPrincipal principal = new CustomPrincipal(new GenericIdentity("Anonymous"), new[] {role});
            operationContext.IncomingMessageProperties.Add("Principal", principal);
            return true;
        }
    }
}