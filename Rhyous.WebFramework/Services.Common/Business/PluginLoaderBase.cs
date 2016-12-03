using Rhyous.SimplePluginLoader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public abstract class PluginLoaderBase<T> where T : class
    {
        public const string PluginDirConfig = "PluginDirectory";

        public static string AppRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string Company = "Rhyous";
        public static string AppName = "Auth.TokenService";
        public static string PluginFolder = "Plugins";

        public abstract string PluginSubFolder { get; }

        public string DefaultPluginDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Company, AppName, PluginFolder);

        public PluginCollection<T> PluginCollection { get; internal set; } // Set exposed as internal for unit tests

        public ILoadPlugins<T> PluginLoader
        {
            get { return _PluginLoader ?? new PluginLoader<T>(Path.Combine(ConfigurationManager.AppSettings.Get(PluginDirConfig, DefaultPluginDirectory), PluginSubFolder)); }
            internal set { _PluginLoader = value; } // Set exposed as internal for unit tests
        } private ILoadPlugins<T> _PluginLoader;

        public List<T> Plugins
        {
            get
            {
                var pluginLibraries = PluginCollection ?? GetPluginLibraries();
                return pluginLibraries.SelectMany(p => p.PluginObjects).ToList();
            }
        } private List<T> _Plugins;

        private PluginCollection<T> GetPluginLibraries()
        {
            var plugins = PluginLoader.LoadPlugins();
            if (plugins == null || plugins.Count == 0)
                throw new Exception($"No {PluginSubFolder} plugin not found.");
            return plugins;
        }
    }
}