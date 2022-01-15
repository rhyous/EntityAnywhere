using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class RestEndpointBehavior : WebHttpBehavior
    {
        internal ILogger Logger;
        public RestEndpointBehavior()
        {
        }
        public RestEndpointBehavior(ILogger logger)
        {
            Logger = logger;
            HelpEnabled = true;
            DefaultOutgoingRequestFormat = DefaultOutgoingResponseFormat = WebMessageFormat.Json;
        }

        public override void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var entityType = GetEntityFromEndpoint(endpoint);
            if (entityType != null)
            {
                foreach (OperationDescription od in endpoint.Contract.Operations)
                    SetUriTemplate(entityType, od);
            }
            base.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        }

        public override WebMessageFormat DefaultOutgoingResponseFormat { get; set; }

        public override WebMessageFormat DefaultOutgoingRequestFormat { get; set; }

        internal void SetUriTemplate(Type entityType, OperationDescription od)
        {
            var webInvokeAttribute = od.OperationBehaviors[typeof(WebInvokeAttribute)] as WebInvokeAttribute;
            if (string.IsNullOrWhiteSpace(webInvokeAttribute.UriTemplate)) // If the UriTemplate is specified, use it as is.
            {
                var uriMethod = CustomUriTemplateDictionary.GetValueOrDefault(od.Name);
                webInvokeAttribute.UriTemplate = uriMethod.Invoke(entityType, od.Name);
            }
        }

        internal Type GetEntityFromEndpoint(ServiceEndpoint endpoint)
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

        protected override IDispatchMessageFormatter GetRequestDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            var parentFormatter = base.GetRequestDispatchFormatter(operationDescription, endpoint);
            return new CustomDispatchMessageFormatter(this, operationDescription, parentFormatter);
        }

        protected override IDispatchMessageFormatter GetReplyDispatchFormatter(OperationDescription operationDescription, ServiceEndpoint endpoint)
        {
            var parentFormatter = base.GetReplyDispatchFormatter(operationDescription, endpoint);
            return new CustomDispatchMessageFormatter(this, operationDescription, parentFormatter);
        }

        internal IDictionaryDefaultValueProvider<string, Func<Type, string, string>> CustomUriTemplateDictionary
        {
            get { return _CustomUriTemplateDictionary ?? (_CustomUriTemplateDictionary = CustomRestDictionary.Instance); }
            set { _CustomUriTemplateDictionary = value; }
        } private IDictionaryDefaultValueProvider<string, Func<Type, string, string>> _CustomUriTemplateDictionary;
    }
}