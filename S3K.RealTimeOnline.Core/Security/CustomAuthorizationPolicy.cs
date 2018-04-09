using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

namespace S3K.RealTimeOnline.Core.Security
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
            IPrincipal principal;
            if (OperationContext.Current.IncomingMessageProperties.ContainsKey("Principal"))
            {
                principal = OperationContext.Current.IncomingMessageProperties["Principal"] as IPrincipal;
            }
            else
            {
                principal = Thread.CurrentPrincipal;
            }

            if (principal != null)
            {
                //do stuff with principal
                evaluationContext.Properties["Principal"] = principal;
                evaluationContext.Properties["Identities"] = new List<IIdentity> { principal.Identity };
                return true;
            }

            return false;
        }
    }
}