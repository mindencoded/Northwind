using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;

namespace S3K.RealTimeOnline.Core.Security
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        private readonly string _id;

        public CustomAuthorizationPolicy()
        {
            _id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get { return _id; }
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            CustomAuthState customstate;

            // If the state is null, then this has not been called before so 
            // set up a custom state.
            if (state == null)
            {
                customstate = new CustomAuthState();
                state = customstate;
            }
            else
            {
                customstate = (CustomAuthState) state;
            }

            // If claims have not been added yet...
            if (!customstate.ClaimsAdded)
            {
                // Create an empty list of claims.
                IList<Claim> claims = new List<Claim>();

                // Iterate through each of the claim sets in the evaluation context.
                foreach (ClaimSet cs in evaluationContext.ClaimSets)
                    // Look for Name claims in the current claimset.
                foreach (Claim c in cs.FindClaims(ClaimTypes.Name, Rights.PossessProperty))
                    // Get the list of operations the given username is allowed to call.
                foreach (string s in GetAllowedOpList(c.Resource.ToString()))
                {
                    // Add claims to the list.
                    claims.Add(new Claim("http://example.org/claims/allowedoperation", s, Rights.PossessProperty));
                    Console.WriteLine(@"Claim added {0}", s);
                }

                // Add claims to the evaluation context.
                if (Issuer != null) evaluationContext.AddClaimSet(this, new DefaultClaimSet(Issuer, claims));

                // Record that claims were added.
                customstate.ClaimsAdded = true;

                // Return true, indicating that this method does not need to be called again.
                return true;
            }

            return false;
        }

        // This method returns a collection of action strings that indicate the 
        // operations the specified username is allowed to call.
        private IEnumerable<string> GetAllowedOpList(string username)
        {
            IList<string> ret = new List<string>();

            if (username == "test1")
            {
                ret.Add("http://Microsoft.ServiceModel.Samples/ICalculator/Add");
                ret.Add("http://Microsoft.ServiceModel.Samples/ICalculator/Multiply");
                ret.Add("http://Microsoft.ServiceModel.Samples/ICalculator/Subtract");
            }
            else if (username == "test2")
            {
                ret.Add("http://Microsoft.ServiceModel.Samples/ICalculator/Add");
                ret.Add("http://Microsoft.ServiceModel.Samples/ICalculator/Subtract");
            }
            return ret;
        }

        // Internal class for keeping track of state.
        class CustomAuthState
        {
            bool _bClaimsAdded;

            public CustomAuthState()
            {
                _bClaimsAdded = false;
            }

            public bool ClaimsAdded
            {
                get { return _bClaimsAdded; }
                set { _bClaimsAdded = value; }
            }
        }
    }
}