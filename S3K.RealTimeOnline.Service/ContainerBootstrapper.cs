using System;
using Microsoft.Practices.Unity;
using S3K.RealTimeOnline.Commons;

namespace S3K.RealTimeOnline.Service
{
    public class ContainerBootstrapper : IContainer
    {
        public ContainerBootstrapper(IUnityContainer container)
        {
            UnityContainer = container;
        }

        public IUnityContainer UnityContainer { get; }

        public T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return UnityContainer.Resolve(type);
        }
    }
}