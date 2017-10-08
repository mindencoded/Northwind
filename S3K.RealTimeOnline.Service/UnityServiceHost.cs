using System;
using System.ServiceModel;
using Microsoft.Practices.Unity;

namespace S3K.RealTimeOnline.Service
{
    internal class UnityServiceHost : ServiceHost
    {
        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Container = container;
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new UnityServiceBehavior(Container));
            }

            base.OnOpening();
        }

        protected IUnityContainer Container
        {
            get;
            set;
        }
    }
}
