using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using Unity;

namespace S3K.RealTimeOnline.Core.Security
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        private IUnityContainer _container;

        public CustomAuthorizationPolicy(IUnityContainer container)
        {
            _container = container;
        }

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
                Debug.WriteLine(string.Format("Identity Name : {0}", principal.Identity.Name));
                return true;
            }

            return false;
        }
    }
}