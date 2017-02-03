using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityEndPointBuilder
    {
        public static void BuildEntityRestService(IEntity entity, WebServiceHostFactory factory = null, RouteCollection routeCollection = null)
        {
            if (routeCollection == null)
                routeCollection = RouteTable.Routes;
            if (factory == null)
                factory = new RestWebServiceHostFactory();
            try
            {
                var entityType = entity.GetType();
                var interfaceType = entityType.GetInterface("I" + entityType.Name);
                var idType = entityType.GetInterface("IEntity`1")?.GetGenericArguments()[0];
                var altIdProp = entityType.GetAlternateIdProperty();
                var webServiceType = (string.IsNullOrWhiteSpace(altIdProp))
                    ? BuildStandardWebService(entityType, interfaceType, idType)
                    : BuildAltIdWebService(entityType, interfaceType, idType, altIdProp);
                routeCollection.Add(new ServiceRoute($"{entityType.Name}Service.svc", factory, webServiceType));
            }
            catch
            {
                throw new Exception($"Could not load web service for entity {entity.GetType().Name}");
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
            var loader = Activator.CreateInstance(entityWebServiceLoaderType);
            var plugins = loader.GetPropertyValue("Plugins") as IList;
            return (plugins != null && plugins.Count > 0) ? plugins[0].GetType() : webServiceBaseType;
        }
    }
}
