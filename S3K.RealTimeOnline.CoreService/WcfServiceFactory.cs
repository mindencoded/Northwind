using Unity;
using Unity.Wcf;

namespace S3K.RealTimeOnline.CoreService
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            ConfigContainer.Instance(container);
        }
    }
}