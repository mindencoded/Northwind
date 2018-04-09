using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace S3K.RealTimeOnline.Core.Security
{
    public class AnonymousAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            IList<string> roles = new List<string>();
            string operationName = operationContext.IncomingMessageProperties["HttpOperationName"].ToString();
            DispatchOperation operation =
                operationContext.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o =>
                    o.Name == operationName);
            if (operation != null)
            {
                Type hostType = operationContext.Host.Description.ServiceType;
                MethodInfo method = hostType.GetMethod(operation.Name);
                if (method != null)
                {
                    PrincipalPermissionAttribute principalPermissionAttribute = method.GetCustomAttributes(true)
                        .Where(a => a is PrincipalPermissionAttribute)
                        .Cast<PrincipalPermissionAttribute>().FirstOrDefault();
                    if (principalPermissionAttribute != null)
                    {
                        string role = principalPermissionAttribute.Role;
                        roles.Add(role);
                    }
                }
            }
            IPrincipal principal = new GenericPrincipal(new GenericIdentity("anonymous"), roles.ToArray());
            Thread.CurrentPrincipal = principal;
            operationContext.IncomingMessageProperties.Add("Principal", principal);
            operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] = principal;
            return true;
        }
    }
}