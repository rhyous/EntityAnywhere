using Autofac;
using Rhyous.Collections;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Behaviors;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.Web.Routing;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RestEndpointBuilder : IEndpointBuilder
    {
        private readonly AttributeToServiceDictionary _AttributeToServiceDictionary;
        private readonly IWebServiceRegistrar _WebServiceRegistrar;
        private readonly IRuntimePluginLoader<IServiceBehavior> _RuntimePluginLoader;
        private readonly IWebServiceLoaderFactory _WebServiceLoaderFactory;
        private readonly ILoadedEntitiesTracker _LoadedEntitiesTracker;
        private readonly ILogger _Logger;

        public RestEndpointBuilder(AttributeToServiceDictionary attributeToServiceDictionary,
                                   IWebServiceRegistrar webServiceRegistrar,
                                   IRuntimePluginLoader<IServiceBehavior> runtimePluginLoader,
                                   IWebServiceLoaderFactory webServiceLoaderFactory,
                                   ILoadedEntitiesTracker loadedEntitiesTracker,
                                   ILogger logger)
        {
            _AttributeToServiceDictionary = attributeToServiceDictionary;
            _WebServiceRegistrar = webServiceRegistrar;
            _RuntimePluginLoader = runtimePluginLoader;
            _WebServiceLoaderFactory = webServiceLoaderFactory;
            _LoadedEntitiesTracker = loadedEntitiesTracker;
            _Logger = logger;
        }

        /// <summary>
        /// Builds a WebService from from a Custom Web Service Plugin.
        /// </summary>
        /// <param name="webServiceType">The custom web service type.</param>
        /// <param name="attribute">The CustomWebServiceAttribute applied to the custom web service type. Uses CustomWebServiceAttribute.ServiceRoute or CustomWebServiceAttribute.Entity to determine the route.</param>
        /// <param name="factory">The WebServiceHostFactory, which is by default the Rhyous.EntityAnywhere.Behaviors.RestWebServiceHostFactory.</param>
        /// <param name="routeCollection">The route collection to update. It is RouteTable.Routes static by default.</param>
        public void BuildCustomEndpoint(Type webServiceType, CustomWebServiceAttribute attribute, WebServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            var serviceRoute = (string.IsNullOrWhiteSpace(attribute.ServiceRoute)) ? $"{attribute.Entity.Name}Service.svc" : attribute.ServiceRoute;
            if (string.IsNullOrWhiteSpace(serviceRoute))
                throw new Exception("Either CustomWebServiceAttribute.ServiceRoute or CustomWebServiceAttribute.Entity must be set.");
            BuildRestEndpoint(webServiceType, serviceRoute, factory, routeCollection);
        }

        /// <summary>
        /// Builds a WebService from an Entity that implements the IEntity interface and is loaded from a dll in the Plugins\Entities directory..
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        public void BuildEntityEndpoint(Type entityType)
        {
            try
            {
                var idTypeAttribute = entityType.GetAttribute<IdTypeAttribute>(true);
                var webServiceTypes = new WebServiceTypesModel
                {
                    EntityType = entityType,
                    InterfaceType = entityType.GetInterface("I" + entityType.Name),
                    IdType = entityType.GetInterface("IId`1")?.GetGenericArguments()[0],
                    AdditionalWebServiceTypes = entityType.GetAdditionalTypes<AdditionalWebServiceTypesAttribute>()
                };
                SetWebServiceTypes(webServiceTypes); // Move this to .Net 4.7 tuple return when we can.
                var webServiceType = BuildWebServiceType(webServiceTypes, idTypeAttribute);
                var name = entityType.Name;
                BuildRestEndpoint(webServiceType, $"{name}Service.svc");
                _Logger.Write($"{entityType} entity endpoint loaded.");
            }
            catch(Exception e)
            {
                throw new Exception($"Could not load web service for entity {entityType.Name}", e);
            }
        }

        /// <summary>
        /// Builds a web service and route for a WCF service.
        /// </summary>
        /// <param name="webServiceType">The custom web service type.</param>
        /// <param name="serviceRoute">The route to provide. Should end with .svc to work.</param>
        /// <param name="factory">The WebServiceHostFactory, which is by default the Rhyous.EntityAnywhere.Behaviors.RestWebServiceHostFactory.</param>
        /// <param name="routeCollection">The route collection to update. It is RouteTable.Routes static by default.</param>
        internal void BuildRestEndpoint(Type webServiceType, string serviceRoute, ServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            if (routeCollection == null)
                routeCollection = RouteTable.Routes;
            if (factory == null)
                factory = new RestWebServiceHostFactory(_RuntimePluginLoader, _Logger);
            routeCollection.Add(new ServiceRoute(serviceRoute, factory, webServiceType));
            // Also add the route without a .svc, unless the route is already without .svc
            var routeWithoutDotSvc = serviceRoute.Replace(".svc", "");
            if (serviceRoute != routeWithoutDotSvc)
                routeCollection.Add(new ServiceRoute(serviceRoute.Replace(".svc", ""), new RestWebServiceHostFactory(_RuntimePluginLoader, _Logger), webServiceType));
            _WebServiceRegistrar.RegisterType(webServiceType);
        }

        internal Type BuildWebServiceType(WebServiceTypesModel types, IdTypeAttribute idTypeAttribute)
        {
            var webServiceTypes = idTypeAttribute == null || idTypeAttribute.IsGenericForWebService
                                ? ArrayMaker.Make(types.EntityType, types.InterfaceType, types.IdType, types.AdditionalWebServiceTypes)
                                : ArrayMaker.Make(types.EntityType, types.InterfaceType, types.AdditionalWebServiceTypes);
            var webServiceBaseType = types.WebServiceGenericType.MakeGenericType(webServiceTypes);

            var loaderTypes = new[] { types.EntityType, webServiceBaseType };
            var entityWebServiceLoaderType = types.LoaderType.MakeGenericType(loaderTypes);

            dynamic loader = _WebServiceLoaderFactory.Create(entityWebServiceLoaderType);
            return (loader.PluginTypes as List<Type>)?.FirstOrDefault() ?? webServiceBaseType;
        }

        internal void SetWebServiceTypes(WebServiceTypesModel webServiceTypes)
        {
            var attributes = webServiceTypes.EntityType.GetCustomAttributes(true)
                                            .Where(a => typeof(EntityAttribute).IsAssignableFrom(a.GetType()));
            foreach (var att in attributes)
            {
                if (_AttributeToServiceDictionary.TryGetValue(att.GetType(), out AttributeServiceTypes types))
                {
                    webServiceTypes.WebServiceGenericType = types.WebServiceGenericType;
                    webServiceTypes.LoaderType = types.LoaderType;
                    return;
                }
            }
            webServiceTypes.WebServiceGenericType = _AttributeToServiceDictionary.DefaultValue.WebServiceGenericType;
            webServiceTypes.LoaderType = _AttributeToServiceDictionary.DefaultValue.LoaderType;
        }
    }
}