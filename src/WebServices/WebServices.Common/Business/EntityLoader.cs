using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Rhyous.WebFramework.WebServices
{
    public class EntityLoader : PluginLoaderBase<IEntity>
    {
        public override string PluginSubFolder => "Entities";

        public static List<Type> LoadedEntities = new List<Type>();

        public static void LoadEntities()
        {
            var entityLoader = new EntityLoader();
            foreach (var item in entityLoader.PluginTypes)
            {
                if (LoadedEntities.Contains(item) || LoadedEntities.Any(t=>t.Name == item.Name))
                    continue;// Entity is alread loaded
                LoadedEntities.Add(item);
                EntityEndPointBuilder.BuildEntityRestService(item);
            }
            RouteTable.Routes.Add(new ServiceRoute("Service", new WebServiceHostFactory(), typeof(MetadataService)));
        }
    }

}