using System;
using Microsoft.Practices.Unity;
using S3K.RealTimeOnline.Commons;
namespace S3K.RealTimeOnline.Service
{
    public class ContainerBootstrapper : IContainer
    {
        private IUnityContainer _container;

        public ContainerBootstrapper(IUnityContainer container)
        {
            _container = container;
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public IUnityContainer UnityContainer { get { return _container; } }
    }
}
