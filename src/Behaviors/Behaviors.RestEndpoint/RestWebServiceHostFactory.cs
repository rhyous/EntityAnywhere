using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Attributes;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Dispatcher;
using System.Collections.Generic;
using System.ServiceModel.Description;
using Rhyous.WebFramework.Services;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestWebServiceHostFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new RestWebServiceHost(serviceType, baseAddresses);
            if (host.ImplementedContracts.Count > 1)
            {
                ConsolidateToSingleContract(serviceType, host);
            }
            var pluginLoader = new PluginLoader<IDispatchMessageInspector>();
            var serviceBehaviorLoader = new ServiceBehaviorLoader();
            if (serviceBehaviorLoader.Plugins?.Count > 0)
            {
                var type = serviceType.GetStaticPropertyValue("EntityType") as Type ?? serviceType;
                AddServiceBehaviorPlugin(type, host.Description.Behaviors, serviceBehaviorLoader.Plugins);
            }
            return host;
        }

        private static void ConsolidateToSingleContract(Type serviceType, RestWebServiceHost host)
        {
            var attribute = serviceType.GetCustomAttributes(true).FirstOrDefault(a => typeof(CustomWebServiceAttribute).IsAssignableFrom(a.GetType())) as CustomWebServiceAttribute;
            if (attribute != null && attribute.ServiceContract != null)
            {
                var keysToRemove = host.ImplementedContracts.Keys.Where(k => k != attribute.ServiceContract.FullName).ToList();
                foreach (var key in keysToRemove)
                {
                    host.ImplementedContracts.Remove(key);
                }
            }
            var contracts = host.ImplementedContracts.ToList();
            foreach (var contract in contracts)
            {
                foreach (var otherContract in contracts.Where(c => c.Key != contract.Key))
                {
                    if (contract.Value.ContractType.IsAssignableFrom(otherContract.Value.ContractType))
                    {
                        host.ImplementedContracts.Remove(contract.Key);
                        if (host.ImplementedContracts.Count == 1)
                            break;
                    }
                }
            }
        }

        private void AddServiceBehaviorPlugin(Type entityType, KeyedByTypeCollection<IServiceBehavior> behaviors, List<IServiceBehavior> serviceBehaviorPlugins)
        {
            var includedAttribute = Attribute.GetCustomAttribute(entityType, typeof(IncludedServiceBehaviorsAttribute)) as IncludedServiceBehaviorsAttribute;
            var excludedAttribute = Attribute.GetCustomAttribute(entityType, typeof(ExcludedServiceBehaviorsAttribute)) as ExcludedServiceBehaviorsAttribute;
            if (includedAttribute != null && excludedAttribute != null)
                throw new Exception("Conflicting attributes. ExcludedServiceBehaviorsAttribute cannot be on the same class as IncludedServiceBehaviorsAttribute.");
            bool hasAttribute = (includedAttribute != null || excludedAttribute != null);
            bool include = includedAttribute != null;
            foreach (var serviceBehavior in serviceBehaviorPlugins)
            {
                if (!hasAttribute
                    || (include && includedAttribute.ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb || serviceBehavior.GetType().Name.Replace("ServiceBehavior", "") == sb))
                    || (!include && !excludedAttribute.ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb || serviceBehavior.GetType().Name.Replace("ServiceBehavior", "") == sb)))
                {
                    behaviors.Add(serviceBehavior);
                }
            }
        }
    }
}
