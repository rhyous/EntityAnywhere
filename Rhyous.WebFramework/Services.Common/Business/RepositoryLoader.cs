using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Rhyous.WebFramework.Services
{
    public class RepositoryLoader
    {
        public static T Load<T>(string entityName)
            where T : class
        {
            var pluginLoader = new PluginLoader<T>();
            var plugindir = ConfigurationManager.AppSettings.Get("PluginDirectory", @".\Plugins\Repositories");
            var dir = Path.Combine(plugindir, "Repositories", entityName);
            var plugins = pluginLoader.LoadPlugins(new List<string> { dir });
            return plugins?[0]?.PluginObjects?[0];
        }

        public static IRepository<T, Tinterface> Load<T, Tinterface>()
            where T : class
        {
            var plugindir = ConfigurationManager.AppSettings.Get("PluginDirectory", @".\Plugins\Repositories");

            var dir = Path.Combine(plugindir, "Repositories", "Common");
            var genericPluginLoader = new PluginLoader<IRepository<T, Tinterface>>();
            var genericPlugins = genericPluginLoader.LoadPlugins(new List<string> { dir });
            return (genericPlugins != null && genericPlugins.Count > 0 && genericPlugins[0].PluginObjects != null && genericPlugins[0].PluginObjects.Count > 0)
                ? genericPlugins[0].PluginObjects[0]
                : null;
        }
    }
}
