using System;
using System.ServiceModel.Activation;
using System.Web.Routing;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IEndpointBuilder
    {
        void BuildEntityEndpoint(Type entityType);
        void BuildCustomEndpoint(Type webServiceType, CustomWebServiceAttribute attribute, WebServiceHostFactory factory = null, RouteCollection routeCollection = null);
    }
}