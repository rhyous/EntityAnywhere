using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System.IO;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    /// <summary>
    /// This class loads the custom entity web service layer plugins or the common entity web service if no custom one exists.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TWebService">The custom entity web service layer plugin type.</typeparam>
    public class EntityWebServiceLoader<TEntity, TWebService>
        : RuntimePluginLoaderBase<TWebService>, 
          IEntityWebServiceLoader<TEntity, TWebService>
        where TWebService : class
    {
        public EntityWebServiceLoader(IAppDomain appDomain,
                                      IPluginLoaderSettings settings,
                                      IPluginLoaderFactory<TWebService> pluginLoaderFactory,
                                      IPluginObjectCreator<TWebService> pluginObjectCreator,
                                      IPluginPaths pluginPaths = null,
                                      ILogger logger = null)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, new PluginLoaderLoggerWrapper(logger))
        {
        }

        /// <inheritdoc />
        /// <remarks>The default WebServices plugin directory is cleverly named: WebServices</remarks>
        public override string PluginSubFolder => Path.Combine("WebServices", typeof(TEntity).Name);
    }
}