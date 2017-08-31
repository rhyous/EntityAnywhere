using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityEndPointBuilder
    {
        /// <summary>
        /// Builds a WebService from from a Custom Web Service Plugin.
        /// </summary>
        /// <param name="webServiceType">The custom web service type.</param>
        /// <param name="attribute">The CustomWebServiceAttribute applied to the custom web service type. Uses CustomWebServiceAttribute.ServiceRoute or CustomWebServiceAttribute.Entity to determine the route.</param>
        /// <param name="factory">The WebServiceHostFactory, which is by default the Rhyous.WebFramework.Behaviors.RestWebServiceHostFactory.</param>
        /// <param name="routeCollection">The route collection to update. It is RouteTable.Routes static by default.</param>
        public static void BuildWebService(Type webServiceType, CustomWebServiceAttribute attribute, WebServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            if (!string.IsNullOrWhiteSpace(attribute.ServiceRoute))
                BuildWebService(webServiceType, attribute.ServiceRoute, factory, routeCollection);
            else if (attribute.Entity != null)
                BuildWebService(webServiceType, $"{attribute.Entity.Name}Service.svc", factory, routeCollection);
            else
                throw new Exception("Either CustomWebServiceAttribute.ServiceRoute or CustomWebServiceAttribute.Entity must be set.");
        }

        /// <summary>
        /// Builds a web service and route for a WCF service.
        /// </summary>
        /// <param name="webServiceType">The custom web service type.</param>
        /// <param name="serviceRoute">The route to provide. Should end with .svc to work.</param>
        /// <param name="factory">The WebServiceHostFactory, which is by default the Rhyous.WebFramework.Behaviors.RestWebServiceHostFactory.</param>
        /// <param name="routeCollection">The route collection to update. It is RouteTable.Routes static by default.</param>
        public static void BuildWebService(Type webServiceType, string serviceRoute, WebServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            if (routeCollection == null)
                routeCollection = RouteTable.Routes;
            if (factory == null)
                factory = new RestWebServiceHostFactory();
            routeCollection.Add(new ServiceRoute(serviceRoute, factory, webServiceType));
        }

        /// <summary>
        /// Builds a WebService from an Entity that implements the IEntity interface and is loaded from a dll in the Plugins\Entities directory..
        /// </summary>
        /// <param name="entityType">The entity type.</param>
        public static void BuildEntityRestService(Type entityType)
        {
            try
            {
                var interfaceType = entityType.GetInterface("I" + entityType.Name);
                var idType = entityType.GetInterface("IId`1")?.GetGenericArguments()[0];
                var additionalServiceTypes = entityType.GetAdditionalTypes<AdditionalServiceTypesAttribute>();
                var additionalWebServiceTypes = entityType.GetAdditionalTypes<AdditionalWebServiceTypesAttribute>();
                var types = GetServiceTypes(entityType); // Move this to .Net 4.7 tuple return when we can.
                var webServiceType = BuildWebService(entityType, interfaceType, idType, types.Item1, types.Item2, types.Item3, additionalServiceTypes, additionalWebServiceTypes);
                BuildWebService(webServiceType, $"{entityType.Name}Service.svc");
            }
            catch
            {
                throw new Exception($"Could not load web service for entity {entityType.Name}");
            }
        }

        internal static Type BuildWebService(Type entityType, Type interfaceType, Type idType, Type serviceGenericType, Type webServiceGenericType, Type loaderType, List<Type> additionalServiceTypes, List<Type> additionalWebServiceTypes)
        {
            var serviceTypes = new List<Type>() { entityType, interfaceType, idType };
            if (additionalServiceTypes != null && additionalServiceTypes.Count > 0)
                serviceTypes.AddRange(additionalServiceTypes);
            var serviceType = serviceGenericType.MakeGenericType(serviceTypes.ToArray());

            var webServiceTypes = new List<Type>() { entityType, interfaceType, idType, serviceType };
            if (additionalWebServiceTypes != null && additionalWebServiceTypes.Count > 0)
                webServiceTypes.AddRange(additionalWebServiceTypes);
            var webServiceBaseType = webServiceGenericType.MakeGenericType(webServiceTypes.ToArray());

            webServiceTypes.Insert(0, webServiceBaseType);
            var entityWebServiceLoaderType = loaderType.MakeGenericType(webServiceTypes.ToArray());
            dynamic loader = Activator.CreateInstance(entityWebServiceLoaderType);
            var plugins = loader.Plugins as IList;
            return (plugins != null && plugins.Count > 0) ? plugins[0].GetType() : webServiceBaseType;
        }

        internal static Tuple<Type, Type, Type> GetServiceTypes(Type entityType)
        {
            var attributes = entityType.GetCustomAttributes().Where(a => typeof(EntityAttribute).IsAssignableFrom(a.GetType()));
            Tuple<Type, Type, Type> ret = null;
            foreach (var att in attributes)
            {
                ServiceDictionary.TryGetValue(att.GetType(), out ret);
                if (ret != null)
                {
                    return ret;
                }
            }
            return ServiceDictionary.DefaultValue;
        }

        internal static IDictionaryDefaultValueProvider<Type, Tuple<Type, Type, Type>> ServiceDictionary
        {
            get { return _ServiceDictionary ?? (_ServiceDictionary = AttributeToServiceDictionary.Instance); }
            set { _ServiceDictionary = value; }
        } private static IDictionaryDefaultValueProvider<Type, Tuple<Type, Type, Type>> _ServiceDictionary;
    }
}
