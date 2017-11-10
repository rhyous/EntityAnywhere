using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Services;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Dispatcher;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestWebServiceHostFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new RestWebServiceHost(serviceType, baseAddresses);
            if (host.ImplementedContracts.Count > 1)
            {
                var attribute = serviceType.GetCustomAttributes(true).FirstOrDefault(a => typeof(CustomWebServiceAttribute).IsAssignableFrom(a.GetType())) as CustomWebServiceAttribute;
                ContractConsolidator.ConsolidateToSingleContract(attribute, host.ImplementedContracts);
            }
            var pluginLoader = new PluginLoader<IDispatchMessageInspector>();
            var serviceBehaviorLoader = new ServiceBehaviorLoader();
            if (serviceBehaviorLoader.Plugins?.Count > 0)
            {
                var entityType = serviceType.GetStaticPropertyValue("EntityType", serviceType) as Type;
                var attributes = entityType.GetCustomAttributes(true).Select(o => o as Attribute).ToList();
                ServiceBehaviorApplicator.AddServiceBehavior(attributes, host.Description.Behaviors, serviceBehaviorLoader.Plugins);
            }
            return host;
        }
    }
}
