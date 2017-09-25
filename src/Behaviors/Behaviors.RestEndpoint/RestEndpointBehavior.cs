using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Common;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Behaviors
{
    public class RestEndpointBehavior : WebHttpBehavior
    {
        public RestEndpointBehavior()
        {
            HelpEnabled = true;
        }

        public override void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var entityType = GetEntityFromEndpoint(endpoint);
            if (entityType != null)
            {
                string pluralEntityName = PluralizationDictionary.Instance.GetValueOrDefault(entityType.Name);
                foreach (OperationDescription od in endpoint.Contract.Operations)
                {
                    var webInvokeAttribute = od.OperationBehaviors[typeof(WebInvokeAttribute)] as WebInvokeAttribute;
                    if (string.IsNullOrWhiteSpace(webInvokeAttribute.UriTemplate))
                    {
                        if (entityType.ImplementsGenericInterface(typeof(IMappingEntity<,>)))
                        {
                            if (od.Name == "GetByE1Ids")
                            {
                                var entity1Pluralized = entityType.GetMappedEntity1Pluralized();
                                webInvokeAttribute.UriTemplate = string.Format(RestDictionary.Instance[od.Name], pluralEntityName, entity1Pluralized);
                            }
                            else if (od.Name == "GetByE2Ids")
                            {
                                var entity2Pluralized = entityType.GetMappedEntity2Pluralized();
                                webInvokeAttribute.UriTemplate = string.Format(RestDictionary.Instance[od.Name], pluralEntityName, entity2Pluralized);
                            }
                            else
                                webInvokeAttribute.UriTemplate = string.Format(RestDictionary.Instance[od.Name], pluralEntityName);
                        }
                        else
                            webInvokeAttribute.UriTemplate = string.Format(RestDictionary.Instance[od.Name], pluralEntityName);
                    }
                }
            }
            base.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        }

        public override bool HelpEnabled
        {
            get { return base.HelpEnabled; }
            set { base.HelpEnabled = value; }
        }

        private Type GetEntityFromEndpoint(ServiceEndpoint endpoint)
        {
            // Get by Attribute
            var type = endpoint.Contract.ContractType;
            var attribute = type.GetCustomAttributes(true).FirstOrDefault(a => typeof(CustomWebServiceAttribute).IsAssignableFrom(a.GetType())) as CustomWebServiceAttribute;
            if (attribute != null && attribute.Entity != null)
                return attribute.Entity;

            // Get by first generic Type parameter
            if (type.IsGenericType)
                return type.GenericTypeArguments[0];
            var inheritedInterfaces = type.GetInterfaceInheritance().Where(t => t.IsGenericType).ToList();
            if (inheritedInterfaces != null && inheritedInterfaces.Count > 0)
                return inheritedInterfaces[0].GenericTypeArguments[0];
            return null;
        }
    }
}