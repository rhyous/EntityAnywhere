using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A base class for loading plugins from a default Plugins directory.
    /// </summary>
    /// <typeparam name="T">The type of the plugin to load.</typeparam>
    public abstract class PluginLoaderBase<T> : IPluginLoader<T>
        where T : class
    {
        public const string PluginDirConfig = "PluginDirectory";

        public static string AppRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string Company = "Rhyous";
        public static string AppName = "EntityAnywhere";
        public static string PluginFolder = "Plugins";

        public virtual bool ThrowExceptionIfNoPluginFound => true;

        /// <summary>
        /// The default parent directory for all plugins.
        /// </summary>
        public string DefaultPluginDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Company, AppName, PluginFolder);

        /// <summary>
        /// The subfolder that groups plugins.
        /// </summary>
        public abstract string PluginSubFolder { get; }

        /// <summary>
        /// A group name for the plugins in the subfolder. This is usually the same name as the PluginSubFolder.
        /// </summary>
        public virtual string PluginGroup => PluginSubFolder;

        /// <summary>
        /// Instantiated instances of the plugins. If multiple dlls have plugins, there could be multiple collections. To get a flattened list, use PLugins.
        /// </summary>
        public virtual PluginCollection<T> PluginCollection
        {
            get { return _PluginCollection ?? (_PluginCollection = GetPluginLibraries()); }
            internal set { _PluginCollection = value; } // Set exposed as internal for unit tests
        } private PluginCollection<T> _PluginCollection;

        public virtual ILoadPlugins<T> PluginLoader
        {
            get { return _PluginLoader ?? new PluginLoader<T>(Path.Combine(ConfigurationManager.AppSettings.Get(PluginDirConfig, DefaultPluginDirectory), PluginSubFolder)); }
            internal set { _PluginLoader = value; } // Set exposed as internal for unit tests
        }
        private ILoadPlugins<T> _PluginLoader;

        /// <summary>
        /// Gets only the types of a plugin, not an actual instance.
        /// </summary>
        public virtual List<Type> PluginTypes { get { return _PluginTypes ?? (_PluginTypes = PluginCollection?.SelectMany(p => p.PluginTypes).ToList()); } }
        private List<Type> _PluginTypes;

        /// <summary>
        /// Gets instantiated instances of the plugins. This is a flattened list of plugins.
        /// </summary>
        public virtual List<T> Plugins { get { return _Plugins ?? (_Plugins = PluginCollection?.SelectMany(p => p.PluginObjects).ToList()); } }
        private List<T> _Plugins;

        /// <summary>
        /// This populates PluginCollection.
        /// </summary>
        /// <returns></returns>
        protected virtual PluginCollection<T> GetPluginLibraries()
        {
            var plugins = PluginLoader.LoadPlugins();
            if (plugins == null || plugins.Count == 0)
            {
                if (ThrowExceptionIfNoPluginFound)
                    throw new Exception($"No {PluginSubFolder} plugin found.");
                return null;
            }
            return plugins;
        }
    }
}