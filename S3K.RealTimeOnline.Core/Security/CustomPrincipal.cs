using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace S3K.RealTimeOnline.Core.Security
{
    public class CustomPrincipal : IPrincipal
    {
        private IIdentity _identity;
        private string[] _roles;

        public CustomPrincipal(IIdentity identity)
        {
            _identity = identity;
        }

        public static CustomPrincipal Current
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public string[] Roles
        {
            get
            {
                if (_roles == null)
                {
                    EnsureRoles();
                }

                return _roles;
            }
        }

        public bool IsInRole(string role)
        {
            EnsureRoles();
            return _roles.Contains(role);
        }

        protected virtual void EnsureRoles()
        {
            ////UserManager userManager = new UserManager();
            ////int userPermissions = userManager.UserPermissions(_identity.Name);

            ////if (userPermissions == 1)
            ////    _roles = new [] { "ADMIN" };
            ////else
            ////    _roles = new [] { "USER" };

            if (_identity.Name == "AnhDV")
                _roles = new[] {"ADMIN"};
            else
                _roles = new[] {"USER"};
        }
    }
}


/*
     class CustomPrincipal : IPrincipal
    {
        IIdentity _identity;
        string[] _roles;

        public CustomPrincipal(IIdentity identity)
        {
            _identity = identity;
        }

        // helper method for easy access (without casting)
        public static CustomPrincipal Current
        {
            get
            {
                return Thread.CurrentPrincipal as CustomPrincipal;
            }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        // return all roles
        public string[] Roles
        {
            get
            {
                EnsureRoles();
                return _roles;
            }
        }

        // IPrincipal role check
        public bool IsInRole(string role)
        {
            EnsureRoles();

            return _roles.Contains(role);
        }

        // read Role of user from database
        protected virtual void EnsureRoles()
        {
            if (_identity.Name == "AnhDV")
                _roles = new string[1] { "ADMIN" };
            else
                _roles = new string[1] { "USER" };
        }
    }
     */