using Rhyous.SimplePluginLoader;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class ServiceBehaviorLoader : RuntimePluginLoaderBase<IServiceBehavior>
    {
        #region Constructor
        public ServiceBehaviorLoader(IAppDomain appDomain,
                                     IPluginLoaderSettings settings,
                                     IPluginLoaderFactory<IServiceBehavior> pluginLoaderFactory,
                                      IPluginObjectCreator<IServiceBehavior> pluginObjectCreator,
                                      IPluginPaths pluginPaths = null,
                                      IPluginLoaderLogger pluginLoaderLogger = null)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        { }
        #endregion

        public override string PluginSubFolder => "ServiceBehaviors";
    }
}