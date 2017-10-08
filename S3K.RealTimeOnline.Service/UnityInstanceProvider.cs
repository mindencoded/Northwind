using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

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
            Container.Teardown(instance);
        }

        protected Type ServiceType
        {
            get;
            set;
        }

        private IUnityContainer Container
        {
            get;
        }
    }
}
