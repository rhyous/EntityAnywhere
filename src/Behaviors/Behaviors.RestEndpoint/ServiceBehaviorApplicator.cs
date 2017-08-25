using Rhyous.WebFramework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;

namespace Rhyous.WebFramework.Behaviors
{
    internal class ServiceBehaviorApplicator
    {
        internal static List<Type> Types = new List<Type>
                                    {
                                        typeof(IncludedServiceBehaviorsAttribute),
                                        typeof(IncludedServiceBehaviorTypesAttribute),
                                        typeof(ExcludedServiceBehaviorsAttribute),
                                        typeof(ExcludedServiceBehaviorTypesAttribute)
                                    };

        internal static void AddServiceBehavior(IList<Attribute> attributes, KeyedByTypeCollection<IServiceBehavior> behaviors, List<IServiceBehavior> serviceBehaviorPlugins)
        {
            if (attributes == null || attributes.Count == 0 || attributes.Count(a => Types.Contains(a.GetType())) == 0)
            {
                serviceBehaviorPlugins.ForEach(sb => behaviors.Add(sb));
                return;
            }
            if (attributes.Count(a => Types.Contains(a.GetType())) > 1)
                throw new ConflictingAttributesException(Types);

            var attribute = attributes.Single(a => Types.Contains(a.GetType()));
            foreach (var serviceBehavior in serviceBehaviorPlugins)
            {
                var serviceBehaviorBase = serviceBehavior as ServiceBehaviorBase;
                if (serviceBehaviorBase == null)
                {
                    if (IsIncluded(serviceBehavior, attribute) || !IsExcluded(serviceBehavior, attribute))
                        behaviors.Add(serviceBehavior);
                }
                if (IsIncluded(serviceBehaviorBase, attribute) || !IsExcluded(serviceBehaviorBase, attribute))
                    behaviors.Add(serviceBehavior);
            }
        }

        internal static bool IsIncluded(ServiceBehaviorBase serviceBehavior, object attribute)
        {
            return (attribute is IncludedServiceBehaviorsAttribute && (attribute as IncludedServiceBehaviorsAttribute).ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb || serviceBehavior.GetType().Name.Replace("ServiceBehavior", "") == sb)
                 || attribute is IncludedServiceBehaviorTypesAttribute && (attribute as IncludedServiceBehaviorTypesAttribute).Types.Any(sb => serviceBehavior.Type == sb)
            );
        }

        internal static bool IsExcluded(ServiceBehaviorBase serviceBehavior, object attribute)
        {
            return (attribute is ExcludedServiceBehaviorsAttribute && (attribute as ExcludedServiceBehaviorsAttribute).ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb || serviceBehavior.GetType().Name.Replace("ServiceBehavior", "") == sb)
                 || attribute is ExcludedServiceBehaviorTypesAttribute && (attribute as ExcludedServiceBehaviorTypesAttribute).Types.Any(sb => serviceBehavior.Type == sb)
            );
        }

        internal static bool IsIncluded(IServiceBehavior serviceBehavior, object attribute)
        {
            return (attribute is IncludedServiceBehaviorsAttribute && (attribute as IncludedServiceBehaviorsAttribute).ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb || serviceBehavior.GetType().Name.Replace("ServiceBehavior", "") == sb)
                 || attribute is IncludedServiceBehaviorTypesAttribute && (attribute as IncludedServiceBehaviorTypesAttribute).Types.Any(sb => ServiceBehaviorType.None == sb)
            );
        }

        internal static bool IsExcluded(IServiceBehavior serviceBehavior, object attribute)
        {
            return (attribute is ExcludedServiceBehaviorsAttribute && (attribute as ExcludedServiceBehaviorsAttribute).ServiceBehaviors.Any(sb => serviceBehavior.GetType().FullName == sb || serviceBehavior.GetType().Name == sb || serviceBehavior.GetType().Name.Replace("ServiceBehavior", "") == sb)
                 || attribute is ExcludedServiceBehaviorTypesAttribute && (attribute as ExcludedServiceBehaviorTypesAttribute).Types.Any(sb => ServiceBehaviorType.None == sb)
            );
        }
    }
}