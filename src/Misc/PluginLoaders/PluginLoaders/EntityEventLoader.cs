using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    /// <summary>
    /// This class loads the custom entity event plugins.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class EntityEventLoader<TEntity, TId> : RuntimePluginLoaderBase<IEntityEvent<TEntity>>
        where TEntity : class, IId<TId>
    {
        private readonly ILogger _Logger2;

        public EntityEventLoader(IAppDomain appDomain,
                                 IPluginLoaderSettings settings,
                                 IPluginLoaderFactory<IEntityEvent<TEntity>> pluginLoaderFactory,
                                 IPluginObjectCreator<IEntityEvent<TEntity>> pluginObjectCreator,
                                 IPluginPaths pluginPaths,
                                 IPluginLoaderLogger pluginLoaderLogger,
                                 ILogger logger)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        {
            _Logger2 = logger;
        }

        /// <inheritdoc />
        /// <remarks>The default entity event plugin directory is cleverly named: Events\Entity</remarks>
        public override string PluginSubFolder => Path.Combine("Events", typeof(TEntity).Name);

        /// <summary>
        /// This method loads the custom entity event plugin.
        /// </summary>
        /// <returns>custom entity event plugin.</returns>
        public IEntityEventAll<TEntity, TId> LoadPlugins()
        {
            var plugins = CreatePluginObjects();
            if (plugins == null || !plugins.Any())
                return null;
            if (plugins.Count > 1)
                _Logger2.Debug($"Found more than one event plugins for {typeof(TEntity).Name}.");

            AddLogger(plugins);
            if (plugins[0] is IEntityEventAll<TEntity, TId> entityEvent)
            {
                var eventContainer = new EntityEventContainer<TEntity, TId>(entityEvent, entityEvent, 
                                entityEvent, entityEvent, entityEvent, entityEvent, entityEvent, 
                                entityEvent, entityEvent, entityEvent, entityEvent, entityEvent,
                                entityEvent, entityEvent);
                return new EntityEventWrapper<TEntity, TId>(eventContainer, new TryLog(_Logger2));
            }
            return BuildEntityEvent(plugins);
        }

        private void AddLogger(IEnumerable<IEntityEvent<TEntity>> plugins)
        {
            foreach (var plugin in plugins)
            {
                if (plugin is ILogProperty loggingPlugin)
                    loggingPlugin.Logger = _Logger2;
            }
        }

        /// <summary>
        /// This builds the entity event from multiple plugins.
        /// </summary>
        /// <returns></returns>
        private IEntityEventAll<TEntity, TId> BuildEntityEvent(IEnumerable<IEntityEvent<TEntity>> plugins)
        {
            var entityEventTypes = new EntityEventTypes<TEntity, TId>();
            var dict = new Dictionary<Type, IEntityEvent<TEntity>>();
            foreach (var type in entityEventTypes.Types)
            {
                dict.Add(type, null);
                foreach (var plugin in plugins)
                {
                    if (type.IsAssignableFrom(plugin.GetType()))
                        dict[type] = plugin;
                }
            }
            var container = new EntityEventContainer<TEntity, TId>(dict, new EntityEventTypes<TEntity, TId>());
            return new EntityEventWrapper<TEntity, TId>(container, new TryLog(_Logger2));
        }
    }
}