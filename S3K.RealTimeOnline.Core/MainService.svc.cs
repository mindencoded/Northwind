using Unity;

namespace S3K.RealTimeOnline.Core
{
    public partial class MainService : BaseService
    {
        private readonly IUnityContainer _container;

        public MainService(IUnityContainer container)
        {
            _container = container;
        }
    }
}