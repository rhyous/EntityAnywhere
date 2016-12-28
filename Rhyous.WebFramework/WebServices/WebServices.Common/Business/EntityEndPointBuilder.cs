using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
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
                var serviceType = typeof(ServiceCommon<,,>).MakeGenericType(entityType, interfaceType, idType);
                var entityWebServiceLoaderType = typeof(EntityWebServiceLoader<,,,>).MakeGenericType(entityType, interfaceType, idType, serviceType);
                var loader = Activator.CreateInstance(entityWebServiceLoaderType);
                var plugins = loader.GetPropertyValue("Plugins") as IList;
                var webServiceType = (plugins != null && plugins.Count > 0)
                    ? plugins[0].GetType()
                    : typeof(EntityWebService<,,,>).MakeGenericType(entityType, interfaceType, idType, serviceType);
                var entityAttributes = entityType.GetCustomAttributes(true);
                routeCollection.Add(new ServiceRoute($"{entityType.Name}Service.svc", factory, webServiceType));
            }
            catch
            {
                throw new Exception($"Could not load web service for entity {entity.GetType().Name}");
            }
        }
    }
}
