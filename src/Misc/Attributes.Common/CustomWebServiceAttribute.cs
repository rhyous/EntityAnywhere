using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// The CustomWebServiceAttribute. This must be used by all custom web services plugin.
    /// </summary>
    public class CustomWebServiceAttribute : Attribute, IExplicitServiceContract
    {
        public CustomWebServiceAttribute(string name = null, Type serviceContract = null, Type entity = null, string serviceRoute = null)
        {
            Name = name;
            ServiceContract = serviceContract;
            Entity = entity;
            ServiceRoute = serviceRoute;
        }

        /// <summary>
        /// The Name of the custom web service. This is unused for anything but a visual aid when looking at the attribute.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the service contract. Only one ServiceContract can be used.
        /// If a web service inherits multiple interfaces that decorated with the ServiceContractAttribute, perhaps due to WebService inheritance, the service contract to use can be specified.
        /// If a service contract is not specified, 
        /// </summary>
        public Type ServiceContract { get; set; }

        /// <summary>
        /// If the custom webservice is for an Entity, specify what entity it is for here.
        /// One of either ServiceRoute or Entity must be set.
        /// </summary>
        public Type Entity { get; set; }

        /// <summary>
        /// If you want a route other than EntityNameService.svc, specify it here. It must end with .svc.
        /// One of either ServiceRoute or Entity must be set.
        /// </summary>
        public string ServiceRoute { get; set; }
    }
}
