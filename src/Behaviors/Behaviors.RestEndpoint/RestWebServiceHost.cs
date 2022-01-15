using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class RestWebServiceHost : WebServiceHost
    {
        private readonly ICustomBindingProvider _CustomBindingProvider;

        public static int FourMegabytes = 4194304;

        public ILogger Logger;
        public Type _ServiceType;

        public RestWebServiceHost(Type serviceType,
                                  ICustomBindingProvider customBindingProvider,
                                  ILogger logger, 
                                  params Uri[] baseAddresses) : base(serviceType, baseAddresses)
        {
            _ServiceType = serviceType;
            _CustomBindingProvider = customBindingProvider;
            Logger = logger;
        }

        protected override void OnOpening()
        {
            base.OnOpening();
        }

        public override void AddServiceEndpoint(ServiceEndpoint endpoint)
        {            
            endpoint.EndpointBehaviors.Clear();
            var entityName = (_ServiceType.GetStaticPropertyValue("EntityType", _ServiceType) as Type)?.Name;
            var providedBinding = _CustomBindingProvider.Get(entityName, endpoint.Binding.Scheme);
            if (providedBinding != null)
            {
                Logger?.Debug($"{entityName}: Provided custom binding found. Default binding will be overwritten.");
                endpoint.Binding = providedBinding;
            }
            if (endpoint.Binding is WebHttpBinding webHttpBinding)
            {
                webHttpBinding.ContentTypeMapper = new JsonContentTypeMapper();
                if (providedBinding == null)
                   webHttpBinding.MaxReceivedMessageSize = FourMegabytes;
            }
            endpoint.EndpointBehaviors.Add(new RestEndpointBehavior(Logger));
            base.AddServiceEndpoint(endpoint);
        }       

        public new IDictionary<string, ContractDescription> ImplementedContracts
        {
            get { return base.ImplementedContracts; }
        }
    }
}
