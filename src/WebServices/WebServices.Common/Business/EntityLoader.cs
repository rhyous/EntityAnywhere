using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web.Routing;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This class loads the Entities and passes them to the EntityEndPointBuilder.
    /// </summary>
    public class EntityLoader : PluginLoaderBase<IEntity>
    {
        /// <inheritdoc />
        /// <remarks>The subfolder for Entities is cleverly named: Entities</remarks>
        public override string PluginSubFolder => "Entities";

        /// <summary>
        /// The types of Loaded Entities.
        /// </summary>
        public static List<Type> LoadedEntities = new List<Type>();

        /// <summary>
        /// This method does three tasks:
        /// 1. Loads the Entities, if not already loaded, and passes them to the EntityEndPointBuilder.
        /// 2. Stores each loaded entity type in the LoadedEntities list.
        /// 3. Creates the ServiceRoute for the WCF service in the RouteTable.
        /// </summary>
        public static void LoadEntities()
        {
            var entityLoader = new EntityLoader();
            foreach (var item in entityLoader.PluginTypes)
            {
                if (LoadedEntities.Contains(item) || LoadedEntities.Any(t=>t.Name == item.Name))
                    continue;// Entity is already loaded
                LoadedEntities.Add(item);
                EntityEndPointBuilder.BuildEntityRestService(item);
            }
            RouteTable.Routes.Add(new ServiceRoute("Service", new WebServiceHostFactory(), typeof(MetadataService)));
        }
    }

}