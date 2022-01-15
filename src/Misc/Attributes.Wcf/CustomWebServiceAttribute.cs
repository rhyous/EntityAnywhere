using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// The CustomWebServiceAttribute. This must be used by all custom web services plugin.
    /// </summary>
    public class CustomWebServiceAttribute : Attribute, IExplicitServiceContract
    {
        /// <summary>
        /// An attribute for a custom service.
        /// </summary>
        /// <param name="name">The name of the custom service.</param>
        /// <param name="serviceContract">The type of the service contract.</param>
        /// <param name="entity">The name of an entity this custom web service is for or null if not for a entity.</param>
        /// <param name="serviceRoute">Allows for a route other than EntityNameService.svc. It can end with .svc but doesn't have to.</param>
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

        /// <inheritdoc />
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
