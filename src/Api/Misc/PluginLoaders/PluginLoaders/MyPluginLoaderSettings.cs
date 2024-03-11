using Rhyous.SimplePluginLoader;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    public class MyPluginLoaderSettings : PluginLoaderSettings
    {
        public MyPluginLoaderSettings(IAppSettings appSettings) : base(appSettings)
        {
        }

        /// <summary>
        /// Put your company name here
        /// </summary>
        public override string Company => "Rhyous";

        /// <summary>
        /// Put your Application name here
        /// </summary>
        public override string AppName => "EAF";    
    }
}
