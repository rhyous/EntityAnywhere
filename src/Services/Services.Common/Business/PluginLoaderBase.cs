using Rhyous.SimplePluginLoader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public abstract class PluginLoaderBase<T> : IPluginLoader<T>
        where T : class
    {
        public const string PluginDirConfig = "PluginDirectory";

        public static string AppRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string Company = "Rhyous";
        public static string AppName = "EntityAnywhere";
        public static string PluginFolder = "Plugins";

        public virtual bool ThrowExceptionIfNoPluginFound => true;

        public string DefaultPluginDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Company, AppName, PluginFolder);

        public abstract string PluginSubFolder { get; }

        public virtual string PluginGroup => PluginSubFolder;

        public virtual PluginCollection<T> PluginCollection { get; internal set; } // Set exposed as internal for unit tests

        public virtual ILoadPlugins<T> PluginLoader
        {
            get { return _PluginLoader ?? new PluginLoader<T>(Path.Combine(ConfigurationManager.AppSettings.Get(PluginDirConfig, DefaultPluginDirectory), PluginSubFolder)); }
            internal set { _PluginLoader = value; } // Set exposed as internal for unit tests
        } private ILoadPlugins<T> _PluginLoader;

        public virtual List<Type> PluginTypes
        {
            get
            {
                if (_PluginTypes != null)
                    return _PluginTypes;
                var pluginLibraries = PluginCollection ?? GetPluginLibraries();
                return (_PluginTypes = pluginLibraries?.SelectMany(p => p.PluginTypes).ToList());
            }
        } private List<Type> _PluginTypes;

        public virtual List<T> Plugins
        {
            get
            {
                if (_Plugins != null)
                    return _Plugins;
                var pluginLibraries = PluginCollection ?? GetPluginLibraries();
                return (_Plugins = pluginLibraries?.SelectMany(p => p.PluginObjects).ToList());
            }
        } private List<T> _Plugins;

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