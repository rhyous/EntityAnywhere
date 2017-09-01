using System.Collections.Generic;
using Rhyous.SimplePluginLoader;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// The plugin loader interface.
    /// </summary>
    /// <typeparam name="T">The type of plugin to load.</typeparam>
    public interface IPluginLoader<T>
        where T : class
    {
        /// <summary>
        /// The collection of loaded plugins.
        /// </summary>
        PluginCollection<T> PluginCollection { get; }
        /// <summary>
        /// The plugin loader.
        /// </summary>
        ILoadPlugins<T> PluginLoader { get; }
        /// <summary>
        /// A list of loaded plugins.
        /// </summary>
        List<T> Plugins { get; }
        /// <summary>
        /// The subfolder underneath the Plugins directory to search.
        /// </summary>
        string PluginSubFolder { get; }
    }
}