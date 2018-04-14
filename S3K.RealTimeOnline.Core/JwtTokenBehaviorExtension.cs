using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace S3K.RealTimeOnline.Core
{
    public class JwtTokenBehaviorExtension : BehaviorExtensionElement, IEndpointBehavior
    {
        public override Type BehaviorType
        {
            get { return typeof(JwtTokenBehaviorExtension); }
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new JwtTokenDispatchMessageInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }

        protected override object CreateBehavior()
        {
            return new JwtTokenBehaviorExtension();
        }
    }
}