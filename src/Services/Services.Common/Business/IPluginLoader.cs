using System.Collections.Generic;
using Rhyous.SimplePluginLoader;

namespace Rhyous.WebFramework.Services
{
    public interface IPluginLoader<T>
        where T : class
    {
        PluginCollection<T> PluginCollection { get; }
        ILoadPlugins<T> PluginLoader { get; }
        List<T> Plugins { get; }
        string PluginSubFolder { get; }
    }
}