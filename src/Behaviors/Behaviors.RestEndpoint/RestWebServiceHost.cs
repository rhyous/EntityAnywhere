using System;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestWebServiceHost : WebServiceHost
    {
        public RestWebServiceHost() : base()
        {
        }

        public RestWebServiceHost(object singletonInstance, params Uri[] baseAddresses) : base(singletonInstance, baseAddresses)
        {
        }

        public RestWebServiceHost(Type serviceType, params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            base.OnOpening();
        }

        public override void AddServiceEndpoint(ServiceEndpoint endpoint)
        {
            endpoint.EndpointBehaviors.Clear();
            endpoint.EndpointBehaviors.Add(new RestEndpointBehavior());
            base.AddServiceEndpoint(endpoint);
        }

        public new IDictionary<string, ContractDescription> ImplementedContracts
        {
            get { return base.ImplementedContracts; }
        }
    }
}
