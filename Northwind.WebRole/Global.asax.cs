using System;
using System.Web;
using Northwind.WebRole.Security;
using Northwind.WebRole.Utils;

namespace Northwind.WebRole
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            if (AppConfig.EnableSecurity)
            {
                if (AppConfig.EncryptionAlgorithm == "RSA")
                {
                    RsaStore.Add("Northwind");
                }
                else if (AppConfig.EncryptionAlgorithm == "HMAC")
                {
                    HmacStore.Add("Northwind");
                }
                else
                {
                    throw new Exception("The encryption algorithm is not recognized.");
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}