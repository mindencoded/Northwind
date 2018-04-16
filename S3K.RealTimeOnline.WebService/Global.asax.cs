using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using S3K.RealTimeOnline.CommonUtils;

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
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfig.RsaPrivateKeyXml),
                        rsa.ToXmlString(true));
                    File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppConfig.RsaPublicKeyXml),
                        rsa.ToXmlString(false));
                }
                else
                {
                    string hmacSecretKey = Convert.ToBase64String(new HMACSHA256().Key);
                    Configuration config =
                        ConfigurationManager.OpenExeConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            "Web.config"));
                    ConfigurationManager.AppSettings.Set("HmacSecretKey", hmacSecretKey);
                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
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