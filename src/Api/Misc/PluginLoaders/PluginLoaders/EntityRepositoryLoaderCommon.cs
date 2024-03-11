using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;
using System;
using System.Linq;
using System.IO;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// A conditional repository loader that implements the custom or common pattern.
    /// It loads a custom repository if it exists, otherwise it loads the common repository.
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    /// </summary>
    public class EntityRepositoryLoaderCommon<TRepositoryInterface, TEntity, TInterface, TId>
               : RuntimePluginLoaderBase<TRepositoryInterface>, IRepositoryLoaderCommon<TRepositoryInterface, TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
        where TRepositoryInterface : IRepository<TEntity, TInterface, TId>
    {

        public EntityRepositoryLoaderCommon(IAppDomain appDomain,
                                            IPluginLoaderSettings settings,
                                            IPluginLoaderFactory<TRepositoryInterface> pluginLoaderFactory,
                                            IPluginObjectCreator<TRepositoryInterface> pluginObjectCreator,
                                            IPluginPaths pluginPaths,
                                            IPluginLoaderLogger pluginLoaderLogger)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)

        {
        }

        /// <inheritdoc />
        /// <remarks>The default Repository plugin directory is cleverly named: Repositories</remarks>
        public override string PluginSubFolder => Path.Combine("Repositories", "Common");

        /// <summary>
        /// Loads a custom repository if it exists, otherwise it loads the common repository.
        /// </summary>
        /// <returns>A custom repository if it exists, otherwise it loads the common repository</returns>
        public TRepositoryInterface LoadPlugin()
        { 
            // Load custom service
            if (PluginCollection == null || !PluginCollection.Any())
                throw new PluginNotFoundException($"No {PluginSubFolder} plugin found.");
            if (PluginTypes == null || !PluginTypes.Any())
                throw new Exception($"{PluginSubFolder} plugin dll was found but no classes were found that implement or inherit type {typeof(TRepositoryInterface)}.");
            var plugins = CreatePluginObjects();
            if (plugins == null || !plugins.Any())
                throw new Exception($"{PluginSubFolder} plugin found but failed to load.");
            _Logger.WriteLine(PluginLoaderLogLevel.Debug, $"Found common plugin for type {typeof(TRepositoryInterface)} of type {plugins[0].GetType()}");
            return plugins[0];
        }
    }
}