using System.Security.Principal;
using System.ServiceModel;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole.Security
{
    public class AnonymousAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string role = operationContext.GetRoleName();
            string[] roles = {};
            if (role != null)
            {
                roles = new[] {role};
            }
            IPrincipal principal = new CustomPrincipal(new GenericIdentity("Anonymous"), roles);
            operationContext.IncomingMessageProperties.Add("Principal", principal);
            return true;
        }
    }
}