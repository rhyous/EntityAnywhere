using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Linq;

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
            var entityType = endpoint.Contract.ContractType.GetInterfaces().Where(i => i.IsGenericType).FirstOrDefault().GenericTypeArguments[0];
            string pluarlEntityName = PlaralizationDictionary.Instance.GetValueOrDefault(entityType.Name);
            foreach (OperationDescription od in endpoint.Contract.Operations)
            {                
                var webInvokeAttribute = od.OperationBehaviors[typeof(WebInvokeAttribute)] as WebInvokeAttribute;
                webInvokeAttribute.UriTemplate = string.Format(RestDictionary.Instance[od.Name], pluarlEntityName);
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
