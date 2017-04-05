using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Services;
using System;
using System.Collections;
using System.Reflection;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityEndPointBuilder
    {
        public static void BuildWebService(Type webServiceType, CustomWebServiceAttribute attribute, WebServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            if (!string.IsNullOrWhiteSpace(attribute.ServiceRoute))
                BuildWebService(webServiceType, attribute.ServiceRoute, factory, routeCollection);
            else if (attribute.Entity != null)
                BuildWebService(webServiceType, $"{attribute.Entity.Name}Service.svc", factory, routeCollection);
            else
                throw new Exception("Either CustomWebServiceAttribute.ServiceRoute or CustomWebServiceAttribute.Entity must be set.");
        }

        public static void BuildWebService(Type webServiceType, string serviceRoute, WebServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            if (routeCollection == null)
                routeCollection = RouteTable.Routes;
            if (factory == null)
                factory = new RestWebServiceHostFactory();
            routeCollection.Add(new ServiceRoute(serviceRoute, factory, webServiceType));
        }

        public static void BuildEntityRestService(Type entityType)
        {
            try
            {
                var interfaceType = entityType.GetInterface("I" + entityType.Name);
                var idType = entityType.GetInterface("IEntity`1")?.GetGenericArguments()[0];
                var altIdProp = entityType.GetAlternateIdProperty();
                var webServiceType = (string.IsNullOrWhiteSpace(altIdProp))
                    ? BuildStandardWebService(entityType, interfaceType, idType)
                    : BuildAltIdWebService(entityType, interfaceType, idType, altIdProp);
                BuildWebService(webServiceType, $"{entityType.Name}Service.svc");
            }
            catch
            {
                throw new Exception($"Could not load web service for entity {entityType.Name}");
            }
        }

        private static Type BuildStandardWebService(Type entityType, Type interfaceType, Type idType)
        {
            var serviceType = typeof(ServiceCommon<,,>).MakeGenericType(entityType, interfaceType, idType);
            var webServiceBaseType = typeof(EntityWebService<,,,>).MakeGenericType(entityType, interfaceType, idType, serviceType);
            var entityWebServiceLoaderType = typeof(EntityWebServiceLoader<,,,,>).MakeGenericType(webServiceBaseType, entityType, interfaceType, idType, serviceType);
            var loader = Activator.CreateInstance(entityWebServiceLoaderType);
            var plugins = loader.GetPropertyValue("Plugins") as IList;
            return (plugins != null && plugins.Count > 0) ? plugins[0].GetType() : webServiceBaseType;
        }

        private static Type BuildAltIdWebService(Type entityType, Type interfaceType, Type idType, string altIdProperty)
        {
            var serviceType = typeof(ServiceCommonAltId<,,>).MakeGenericType(entityType, interfaceType, idType);
            var webServiceBaseType = typeof(EntityWebServiceAltId<,,,>).MakeGenericType(entityType, interfaceType, idType, serviceType);
            var entityWebServiceLoaderType = typeof(EntityWebServiceLoader<,,,,>).MakeGenericType(webServiceBaseType, entityType, interfaceType, idType, serviceType);
            dynamic loader = Activator.CreateInstance(entityWebServiceLoaderType);
            var plugins = loader.Plugins as IList;
            return (plugins != null && plugins.Count > 0) ? plugins[0].GetType() : webServiceBaseType;
        }
    }
}
