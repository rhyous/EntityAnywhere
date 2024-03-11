using Autofac;
using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.IO;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    /// <summary>
    /// This class loads the custom entity service layer plugins or the common entity service if no custom one exists.
    /// </summary>
    /// <typeparam name="TService">The custom entity service layer plugin type. To use the common entity web service and have a custom entity service, the custom service must inherit ServiceCommon{T, Tinterface, Tid}.</typeparam>
    public class EntityServiceLoaderCommon<TServiceInterface, TEntity, TInterface, TId>
           : RuntimePluginLoaderBase<TServiceInterface>, IEntityServiceLoaderCommon<TServiceInterface, TEntity, TInterface, TId> 
        where TServiceInterface : IServiceCommon<TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
    {
        private readonly ILifetimeScope _Container;

        public EntityServiceLoaderCommon (ILifetimeScope container,
                                          IAppDomain appDomain,
                                          IPluginLoaderSettings settings,
                                          IPluginLoaderFactory<TServiceInterface> pluginLoaderFactory,
                                          IPluginObjectCreator<TServiceInterface> pluginObjectCreator,
                                          IPluginPaths pluginPaths,
                                          IPluginLoaderLogger pluginLoaderLogger)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        {
            _Container = container;
        }

        /// <inheritdoc />
        /// <remarks>The default Services plugin directory is cleverly named: Services</remarks>
        public override string PluginSubFolder => Path.Combine("Services", "Common");

        /// <summary>
        /// This method loads the custom entity service layer plugin or the common entity service if no custom one exists.
        /// </summary>
        /// <returns>The custom or common service.</returns>
        public TServiceInterface LoadPlugin()
        {
            // Load custom service
            if (PluginCollection != null && PluginCollection.Any())
            {
                var plugins = CreatePluginObjects();
                if (plugins == null || !plugins.Any())
                    throw new Exception($"{PluginSubFolder} plugin found but failed to load.");
                return plugins[0];
            }
            throw new PluginNotFoundException($"No PluginSubFolder plugin found.");
        }
    }
}