using Rhyous.SimplePluginLoader;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    /// <summary>
    /// This class loads the Entities and passes them to the EntityEndPointBuilder.
    /// </summary>

    public class EntityLoader : RuntimePluginLoaderBase<IBaseEntity>, IEntityLoader
    {
        private readonly IEntityList _EntityList;
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IMappingEntityList _MappingEntityList;
        private readonly ILogger _Logger2;

        public EntityLoader(IEntityList entityList,
                            IExtensionEntityList extensionEntityList,
                            IMappingEntityList mappingEntityList,
                            IAppDomain appDomain,
                            IPluginLoaderSettings settings,
                            IPluginLoaderFactory<IBaseEntity> pluginLoaderFactory,
                            IPluginObjectCreator<IBaseEntity> pluginObjectCreator,
                            IPluginPaths pluginPaths = null,
                            IPluginLoaderLogger pluginLoaderLogger = null,
                            ILogger logger = null)
            : base(appDomain, settings, pluginLoaderFactory, pluginObjectCreator, pluginPaths, pluginLoaderLogger)
        {
            _EntityList = entityList;
            _ExtensionEntityList = extensionEntityList;
            _MappingEntityList = mappingEntityList;
            _Logger2 = logger;
        }

        /// <inheritdoc />
        /// <remarks>The subfolder for Entities is cleverly named: Entities</remarks>
        public override string PluginSubFolder => "Entities";

        /// <summary>
        /// This method does these tasks:
        /// 1. Passes each loaded to the EntityEndPointBuilder.
        /// 2. Stores each loaded entity type in the LoadedEntities list.
        /// 3. Creates the ServiceRoute for the WCF service in the RouteTable.
        /// </summary>
        public void Load(IEnumerable<Type> entityTypes = null)
        {
            if (entityTypes == null || !entityTypes.Any())
                entityTypes = PluginTypes;
            foreach (var entityType in entityTypes)
            {
                if (_EntityList.Entities.Contains(entityType) || _EntityList.Entities.Any(t => t.Name == entityType.Name))
                {
                    _Logger2?.Write($"{entityType} entity endpoint skipped. It is already loaded.");
                    continue;// Entity is already loaded
                }
                _EntityList.Entities.Add(entityType);
                if (entityType.GetAttribute<ExtensionEntityAttribute>() != null)
                    _ExtensionEntityList.Entities.Add(entityType);
                if (entityType.GetAttribute<MappingEntityAttribute>() != null)
                    _MappingEntityList.Entities.Add(entityType);
            }
        }
    }
}