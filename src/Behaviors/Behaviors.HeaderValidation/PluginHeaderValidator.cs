using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Specialized;

namespace Rhyous.WebFramework.Behaviors
{
    class PluginHeaderValidator : PluginLoaderBase<IHeaderValidator>, IHeaderValidator
    {
        public override string PluginSubFolder => "Authenticators";
        
        public long UserId { get; set; }

        private PluginCollection<IHeaderValidator> GetPlugins()
        {
            var plugins = PluginLoader.LoadPlugins();
            if (plugins == null || plugins.Count == 0)
                throw new Exception("No authenticator plugin not found.");
            return plugins;
        }

        public bool IsValid(NameValueCollection headers)
        {
            var plugins = PluginCollection ?? GetPlugins();
            foreach (var plugin in plugins)
            {
                foreach (var obj in plugin.PluginObjects)
                {
                    if (obj.IsValid(headers))
                    {
                        UserId = obj.UserId;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
