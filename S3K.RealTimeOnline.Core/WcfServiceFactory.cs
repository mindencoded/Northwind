using System;
using System.Linq;
using S3K.RealTimeOnline.Contracts.Services;
using Unity;
using Unity.Wcf;

namespace S3K.RealTimeOnline.Core
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            Type[] serviceTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IService).IsAssignableFrom(p) && !p.IsAbstract).ToArray();
            foreach (var serviceType in serviceTypes)
            {
                Type contractType = serviceType.GetInterfaces()
                    .FirstOrDefault(t => typeof(IService).IsAssignableFrom(t));
                if (contractType != null)
                {
                    container.RegisterType(contractType, serviceType);
                }
            }

            ConfigContainer.Instance(container);
        }
    }
}