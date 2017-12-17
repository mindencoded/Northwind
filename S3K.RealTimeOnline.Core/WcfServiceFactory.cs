using Unity;
using Unity.Wcf;

namespace S3K.RealTimeOnline.Core
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            ConfigContainer.Instance(container);
        }
    }
}