using System;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Northwind.WebRole.Utils
{
    public static class OperationContextExtension
    {
        public static string GetRoleName(this OperationContext operationContext)
        {
            string operationName = operationContext.IncomingMessageProperties["HttpOperationName"].ToString();
            DispatchOperation operation =
                operationContext.EndpointDispatcher.DispatchRuntime.Operations.FirstOrDefault(o =>
                    o.Name == operationName);
            if (operation != null)
            {
                Type serviceType = operationContext.Host.Description.ServiceType;
                MethodInfo[] serviceMethods = serviceType.GetMethods();
                MethodInfo serviceMethod = serviceMethods.FirstOrDefault(m =>m.Name == operation.Name);
                if (serviceMethod == null)
                {
                    MethodInfo contractMethod = serviceType.GetInterfaces()
                        .SelectMany(t => t.GetMethods())
                        .FirstOrDefault(m => ((OperationContractAttribute) m.GetCustomAttribute(typeof(OperationContractAttribute), true)).Name == operation.Name);
                    if (contractMethod != null)
                    {
                        serviceMethod = serviceMethods.FirstOrDefault(m => m.Name == contractMethod.Name 
                                                                           && m.GetParameters().Count() == contractMethod.GetParameters().Count()
                                                                           && !m.GetParameters().Select(p => p.ParameterType).Except(contractMethod.GetParameters().Select(p => p.ParameterType)).Any()
                                                                           );
                    }
                }

                if (serviceMethod != null)
                {
                    PrincipalPermissionAttribute principalPermissionAttribute = serviceMethod.GetCustomAttributes(true)
                        .Where(a => a is PrincipalPermissionAttribute)
                        .Cast<PrincipalPermissionAttribute>().FirstOrDefault();
                    if (principalPermissionAttribute != null)
                    {
                        return principalPermissionAttribute.Role;
                    }
                }
            }

            return null;
        }
    }
}