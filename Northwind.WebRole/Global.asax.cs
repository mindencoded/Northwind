using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using Northwind.WebRole.Contracts;
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

            Type[] serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(p => typeof(IService).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToArray();
            foreach (Type serviceType in serviceTypes)
            {
                RoutePrefixAttribute attribute = serviceType.GetCustomAttribute<RoutePrefixAttribute>(false);
                if (attribute != null)
                {
                    RouteTable.Routes.Add(new ServiceRoute(attribute.Name, new WcfServiceHostFactory(), serviceType));
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