using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Attributes;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Dispatcher;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestWebServiceHostFactory : WebServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var host = new RestWebServiceHost(serviceType, baseAddresses);
            var pluginLoader = new PluginLoader<IDispatchMessageInspector>();
            var serviceBehaviorLoader = new ServiceBehaviorLoader();
            if (serviceBehaviorLoader.Plugins?.Count > 0)
            {
                AddServiceBehaviorPlugin(serviceType.GetProperty("EntityType").GetValue(null) as Type, host.Description.Behaviors, serviceBehaviorLoader.Plugins);
            }
            return host;
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
                    || (include && includedAttribute.ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb))
                    || (!include && !excludedAttribute.ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb)))
                {
                    behaviors.Add(serviceBehavior);
                }
            }
        }
    }
}
