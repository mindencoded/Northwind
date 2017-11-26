using S3K.RealTimeOnline.CoreService.Contracts;
using Unity;
using Unity.Wcf;

namespace S3K.RealTimeOnline.CoreService
{
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container
                .RegisterType<IRestEndPoint, RestEndPoint>();
        }
    }
}