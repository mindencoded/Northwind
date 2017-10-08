using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace S3K.RealTimeOnline.Service
{
    internal class UnityServiceBehavior : IServiceBehavior
    {
        public UnityServiceBehavior(IUnityContainer container)
        {
            Container = container;
        }


        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
            BindingParameterCollection bindingParameters)
        {
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            /*for (int dispatcherIndex = 0; dispatcherIndex < serviceHostBase.ChannelDispatchers.Count; dispatcherIndex++)
            {
                ChannelDispatcherBase dispatcher = serviceHostBase.ChannelDispatchers[dispatcherIndex];
                ChannelDispatcher channelDispatcher = (ChannelDispatcher) dispatcher;
                for (int endpointIndex = 0; endpointIndex < channelDispatcher.Endpoints.Count; endpointIndex++)
                {
                    EndpointDispatcher endpointDispatcher = channelDispatcher.Endpoints[endpointIndex];
                    endpointDispatcher.DispatchRuntime.InstanceProvider =
                        new UnityInstanceProvider(Container, serviceDescription.ServiceType);
                }
            }*/

            foreach (ChannelDispatcherBase cdb in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher cd = (ChannelDispatcher) cdb;

                foreach (EndpointDispatcher ed in cd.Endpoints)
                {
                    ed.DispatchRuntime.InstanceProvider =  new UnityInstanceProvider(Container, serviceDescription.ServiceType);
                }

            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }


        protected IUnityContainer Container { get; set; }
    }
}
