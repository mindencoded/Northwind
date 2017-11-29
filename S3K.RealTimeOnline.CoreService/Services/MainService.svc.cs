using Unity;

namespace S3K.RealTimeOnline.CoreService.Services
{
    public partial class MainService
    {
        private readonly IUnityContainer _container;

        public MainService(IUnityContainer container)
        {
            _container = container;
        }
    }
}