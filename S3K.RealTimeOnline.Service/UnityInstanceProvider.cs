using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Unity;

namespace S3K.RealTimeOnline.Service
{
    internal class UnityInstanceProvider : IInstanceProvider
    {
        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            Container = container;
            ServiceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return Container.Resolve(ServiceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            throw new NotImplementedException();
        }

        protected Type ServiceType { get; set; }

        private IUnityContainer Container { get; }
    }
}