using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
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
    public class EntityRepositoryLoader<TRepositoryInterface, TEntity, TInterface, TId> 
               : RuntimePluginLoaderBase<TRepositoryInterface>, IRepositoryLoader<TRepositoryInterface, TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
        where TRepositoryInterface : IRepository<TEntity, TInterface, TId>
    {
        private readonly IRepositoryLoaderCommon<TRepositoryInterface, TEntity, TInterface, TId> _RepositoryLoaderCommon;

        public EntityRepositoryLoader(IAppDomain appDomain,
                                      IPluginLoaderSettings settings,
                                      IPluginLoaderFactory<TRepositoryInterface> pluginLoaderFactory,
                                      IPluginObjectCreator<TRepositoryInterface> pluginObjectCreator,
                                      IPluginPaths pluginPaths,
                                      IRepositoryLoaderCommon<TRepositoryInterface, TEntity, TInterface, TId> repositoryLoaderCommon,
                                      IPluginLoaderLogger pluginLoaderLogger)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)

        {
            _RepositoryLoaderCommon = repositoryLoaderCommon;
        }

        /// <inheritdoc />
        /// <remarks>The default Repository plugin directory is cleverly named: Repositories</remarks>
        public override string PluginSubFolder => Path.Combine("Repositories", typeof(TEntity).Name);

        /// <summary>
        /// Loads a custom repository if it exists, otherwise it loads the common repository.
        /// </summary>
        /// <returns>A custom repository if it exists, otherwise it loads the common repository</returns>
        public TRepositoryInterface LoadPlugin()
        {
            // Load custom service
            if (PluginCollection != null && PluginCollection.Any())
            {
                var plugins = CreatePluginObjects();
                if (plugins == null || !plugins.Any())
                    throw new Exception($"Custom {PluginSubFolder} plugin found but failed to load.");
                _Logger.WriteLine(PluginLoaderLogLevel.Debug, $"Found custom plugin for type {typeof(TRepositoryInterface)} of type {plugins[0].GetType()}");
                return plugins[0];
            }
            // Load common service
            _Logger.WriteLine(PluginLoaderLogLevel.Debug, $"No custom plugin found for {typeof(TRepositoryInterface)}. Loading common.");
            return _RepositoryLoaderCommon.LoadPlugin();
        }
    }
}