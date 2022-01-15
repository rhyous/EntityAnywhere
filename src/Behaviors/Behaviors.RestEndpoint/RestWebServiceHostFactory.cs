using Autofac.Integration.Wcf;
using Rhyous.Collections;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class RestWebServiceHostFactory : AutofacWebServiceHostFactory
    {
        private readonly IRuntimePluginLoader<IServiceBehavior> _BehaviorLoader;
        internal ILogger Logger;
        public RestWebServiceHostFactory(IRuntimePluginLoader<IServiceBehavior> serviceBehaviorLoader,
                                         ILogger logger)
        {
            _BehaviorLoader = serviceBehaviorLoader;
            Logger = logger;
        }

        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            return CreateServiceHostInternal(serviceType, baseAddresses);
        }

        internal ServiceHost CreateServiceHostInternal(Type serviceType, Uri[] baseAddresses)
        {
            var entityType = serviceType.GetStaticPropertyValue("EntityType", serviceType) as Type;
            RestWebServiceHost host = new RestWebServiceHost(serviceType, new CustomBindingProvider(), Logger, baseAddresses);

            if (host.ImplementedContracts.Count > 1)
            {
                var attribute = serviceType.GetCustomAttributes(true).FirstOrDefault(a => typeof(CustomWebServiceAttribute).IsAssignableFrom(a.GetType())) as CustomWebServiceAttribute;
                ContractConsolidator.ConsolidateToSingleContract(attribute, host.ImplementedContracts);
            }

            SetInstanceContextModeToPerCall(host.Description.Behaviors);
            AddPluginServiceBehaviors(host, entityType);

            return host;
        }

        internal static void SetInstanceContextModeToPerCall(KeyedByTypeCollection<IServiceBehavior> behaviors)
        {
            var serviceBehaviorAttribute = behaviors.Find<ServiceBehaviorAttribute>();
            if (serviceBehaviorAttribute == null)
            {
                serviceBehaviorAttribute = new ServiceBehaviorAttribute();
                behaviors.Add(serviceBehaviorAttribute);
            }
            serviceBehaviorAttribute.InstanceContextMode = InstanceContextMode.PerCall;
            serviceBehaviorAttribute.ConcurrencyMode = ConcurrencyMode.Multiple;
        }

        internal void AddPluginServiceBehaviors(RestWebServiceHost host, Type entityType)
        {
            var plugins = _BehaviorLoader.CreatePluginObjects();
            if (plugins != null)
            {
                foreach (var plugin in plugins)
                {
                    if (plugin is ILogProperty serviceBehaviorBase)
                        serviceBehaviorBase.Logger = Logger;
                }
                if (plugins.Count > 0)
                {
                    Logger?.Debug($"Found {plugins.Count} service behaviors to apply to web service for {entityType.Name} entity: {string.Join(", ", plugins.Select(p=>p.GetType().Name))}");
                    var attributes = entityType.GetCustomAttributes(true).Select(o => o as Attribute).ToList();
                    ServiceBehaviorApplicator.AddServiceBehavior(attributes, host.Description.Behaviors, plugins, Logger);
                }
            }
        }
    }
}