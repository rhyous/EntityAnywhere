using Rhyous.SimplePluginLoader;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;

namespace Rhyous.WebFramework.Behaviors
{
    class PluginTokenValidator : PluginLoaderBase<ITokenValidator>, ITokenValidator
    {
        public override string PluginSubFolder => "Authenticators";

        public IToken Token { get; set; }

        private PluginCollection<ITokenValidator> GetPlugins()
        {
            var plugins = PluginLoader.LoadPlugins();
            if (plugins == null || plugins.Count == 0)
                throw new Exception("No authenticator plugin not found.");
            return plugins;
        }

        public bool IsValid(string token)
        {
            var plugins = PluginCollection ?? GetPlugins();
            foreach (var plugin in plugins)
            {
                foreach (var obj in plugin.PluginObjects)
                {
                    if (obj.IsValid(token))
                    {
                        Token = obj.Token;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
