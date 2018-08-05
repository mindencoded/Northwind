﻿using System;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Northwind.WebRole
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
                Type hostType = operationContext.Host.Description.ServiceType;
                MethodInfo method = hostType.GetMethod(operation.Name);
                if (method != null)
                {
                    PrincipalPermissionAttribute principalPermissionAttribute = method.GetCustomAttributes(true)
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