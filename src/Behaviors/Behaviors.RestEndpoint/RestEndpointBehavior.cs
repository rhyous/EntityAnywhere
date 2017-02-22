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
            var entityType = endpoint.Contract.ContractType.GenericTypeArguments[0];
            string pluralEntityName = PlaralizationDictionary.Instance.GetValueOrDefault(entityType.Name);
            foreach (OperationDescription od in endpoint.Contract.Operations)
            {                
                var webInvokeAttribute = od.OperationBehaviors[typeof(WebInvokeAttribute)] as WebInvokeAttribute;
                webInvokeAttribute.UriTemplate = string.Format(RestDictionary.Instance[od.Name], pluralEntityName);
            }
            base.ApplyDispatchBehavior(endpoint, endpointDispatcher);
        }

        public override bool HelpEnabled
        {
            get { return base.HelpEnabled; }
            set { base.HelpEnabled = value; }
        }
    }
}