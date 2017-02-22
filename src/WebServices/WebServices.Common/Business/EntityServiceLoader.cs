using Rhyous.WebFramework.Services;
using System.IO;

namespace Rhyous.WebFramework.WebServices
{

    public class EntityServiceLoader<T, TService> : PluginLoaderBase<TService>
        where TService : class, new()
    {
        public override bool ThrowExceptionIfNoPluginFound => false;
        public override string PluginSubFolder => Path.Combine("Services", typeof(T).Name);

        public TService LoadPluginOrCommon()
        {
            return (Plugins != null && Plugins.Count > 0) ? Plugins[0] : new TService();
        }
    }
}