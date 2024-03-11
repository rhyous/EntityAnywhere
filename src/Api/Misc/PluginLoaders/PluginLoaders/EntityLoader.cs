using Rhyous.Odata;
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
        private readonly ICustomPluralizer _CustomPluralizer;
        private readonly ILogger _Logger2;

        public EntityLoader(IEntityList entityList,
                            IExtensionEntityList extensionEntityList,
                            IMappingEntityList mappingEntityList,
                            ICustomPluralizer customPluralizer,
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
            _CustomPluralizer = customPluralizer;
            _Logger2 = logger;
        }

        /// <inheritdoc />
        /// <remarks>The subfolder for Entities is cleverly named: Entities</remarks>
        public override string PluginSubFolder => "Entities";

        /// <summary>
        /// This method adds th entities to the appropriate EntityLists.
        /// </summary>
        public void Load(IEnumerable<Type> entityTypes = null)
        {
            if (entityTypes == null || !entityTypes.Any())
                entityTypes = PluginTypes;
            foreach (var entityType in entityTypes)
            {
                UpdatePluralization(entityType);
                _EntityList.Entities.Add(entityType);
                if (entityType.GetAttribute<ExtensionEntityAttribute>() != null)
                    _ExtensionEntityList.Entities.Add(entityType);
                if (entityType.GetAttribute<MappingEntityAttribute>() != null)
                    _MappingEntityList.Entities.Add(entityType);
            }
        }

        private void UpdatePluralization(Type entityType)
        {
            var pluralAttribute = entityType.GetAttribute<PluralNameAttribute>();
            if (pluralAttribute != null)
            {
                foreach (var isoCode in new[] { "en", "en-US" })
                    _CustomPluralizer.AddCustomPluralization(isoCode, entityType.Name, pluralAttribute.PluralName);
            }
        }
    }
}