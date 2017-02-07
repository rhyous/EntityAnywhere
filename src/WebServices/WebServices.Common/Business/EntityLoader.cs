using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityLoader : PluginLoaderBase<IEntity>
    {
        public override string PluginSubFolder => "Entities";

        public static List<string> LoadedEntities = new List<string>();

        public static void LoadEntities()
        {
            var entityLoader = new EntityLoader();
            foreach (var item in entityLoader.Plugins)
            {
                LoadedEntities.Add(item.GetType().Name);
                EntityEndPointBuilder.BuildEntityRestService(item);
            }
            RouteTable.Routes.Add(new ServiceRoute("Service", new WebServiceHostFactory(), typeof(MetadataService)));
        }
    }

}