using Rhyous.SimplePluginLoader;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class MyPluginPaths : IPluginPaths
    {
        private readonly Rhyous.SimplePluginLoader.IAppSettings _AppSettings;
        const string PluginDirectory = "PluginDirectory";

        public MyPluginPaths(Rhyous.SimplePluginLoader.IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }

        public IEnumerable<string> Paths => _Paths ?? (_Paths = new string[] { _AppSettings.Settings.Get(PluginDirectory) });
        private IEnumerable<string> _Paths;
    }
}