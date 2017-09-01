using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Services;
using System;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class WebServiceLoader : PluginLoaderBase<ICustomWebService>
    {
        public override string PluginSubFolder => "WebServices";

        public static void LoadEntities()
        {
            var loader = new WebServiceLoader();
            var customWebServiceTypes = loader.PluginTypes.Where(t => !t.IsInterface).ToList();
            foreach (Type item in customWebServiceTypes)
            {
                var attribute = item.GetCustomAttributes(true)?.FirstOrDefault(a => typeof(CustomWebServiceAttribute).IsAssignableFrom(a.GetType())) as CustomWebServiceAttribute;
                if (attribute != null)
                {
                    if (attribute.Entity != null)
                        EntityLoader.LoadedEntities.Add(attribute.Entity);
                    EntityEndPointBuilder.BuildWebService(item, attribute);
                }
            }
        }
    }
}