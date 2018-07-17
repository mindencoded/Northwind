using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading;

namespace Northwind.WebRole.Security
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        private readonly Guid _id = Guid.NewGuid();

        public string Id
        {
            get { return _id.ToString(); }
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            object value;
            if (OperationContext.Current.IncomingMessageProperties.TryGetValue("Principal", out value))
            {
                IPrincipal principal = value as IPrincipal;
                evaluationContext.Properties["Principal"] = principal;
                Thread.CurrentPrincipal = principal;
                return true;
            }

            throw new WebFaultException(HttpStatusCode.Unauthorized);
        }
    }
}