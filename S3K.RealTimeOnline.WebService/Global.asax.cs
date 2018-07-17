using System;
using System.Web;
using S3K.RealTimeOnline.CommonUtils;
using S3K.RealTimeOnline.WebService.Security;

namespace S3K.RealTimeOnline.WebService
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            if (AppConfig.EnableSecurity)
            {
                if (AppConfig.UseRsa)
                {
                    RsaStore.Add("Custom");
                }
                else
                {
                    HmacStore.Add("Custom");
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