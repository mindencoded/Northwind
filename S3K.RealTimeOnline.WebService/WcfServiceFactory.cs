using S3K.RealTimeOnline.Core;
using Unity;
using Unity.Wcf;

namespace S3K.RealTimeOnline.WebService
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            ConfigContainer configContainer = new ConfigContainer();
            configContainer.Init(container);
        }
    }
}