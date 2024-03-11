using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{

    /// <summary>
    /// This class determines whether each Entity, EntityProperty, or EntityGoup already exists or not.
    /// </summary>
    public class MissingEntitySettingDetector : IMissingEntitySettingDetector
    {
        private const string DefaultGroup = "Miscellaneous";

        private readonly Type[] _EnitityDiscriptionAttributes = new[] { typeof(EntitySettingsAttribute), typeof(DescriptionAttribute) };
        private readonly IEntityGroupCache _EntitySettingsGroupCache;
        private readonly ITypeInfoResolver _EntityInfoResolver;

        public MissingEntitySettingDetector(IEntityGroupCache entitySettingsGroupCache,
                                            ITypeInfoResolver entityInfoResolver)
        {
            _EntitySettingsGroupCache = entitySettingsGroupCache;
            _EntityInfoResolver = entityInfoResolver;
        }

        public async Task<MissingEntitySettings> DetectAsync(IEntitySettingsDictionary settings, IEnumerable<Type> entityTypes)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (entityTypes == null) throw new ArgumentNullException(nameof(entityTypes));
            var missingSettings = new MissingEntitySettings();
            foreach (var entityType in entityTypes)
            {
                if (!settings.TryGetValue(entityType.Name, out EntitySettings entity))
                {
                    await AddEntitySettingFromTypeAsync(missingSettings, entityType);
                    continue;
                }
                AddMissingProperties(missingSettings, entityType, entity);
            }
            return missingSettings;
        }

        private void AddMissingProperties(MissingEntitySettings missingSettings, Type entityType, EntitySettings missingEntity)
        {
            var entityWithMissingProperties = missingEntity.ToEntityWithMissingProperties(entityType);
            var missing = new Missing<EntityWithMissingProperties>(entityWithMissingProperties);

            var entityInfo = _EntityInfoResolver.Resolve(entityType);
            foreach (var kvp in entityInfo.Properties)
            {
                // If property should be excluded from Metadata, don't include it.
                var propInfo = kvp.Value;
                if (propInfo.ExcludeFromMetadata())
                    continue; 
                if (!missingEntity.EntityProperties.TryGetValue(propInfo.Name, out EntityProperty prop))
                {
                    missingSettings.Entities.AddIfNew(missingEntity.Entity.Name, missing);
                    AddFromPropertyInfo(missing, propInfo);
                }
            }
        }

        internal async Task AddEntitySettingFromTypeAsync(MissingEntitySettings missingSettings, Type entityType)
        {
            if (missingSettings == null) throw new ArgumentNullException(nameof(missingSettings));
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            var entity = new Missing<EntityWithMissingProperties>
            {
                Object = new EntityWithMissingProperties
                {
                    Entity = new Entity
                    {
                        Name = entityType.Name,
                        Description = entityType.GetAttributePropertyValue(_EnitityDiscriptionAttributes, "Description", ""),
                        Enabled = true,
                        EntityGroupId = 1
                    }
                },
                IsMissing = true
            };
            missingSettings.Entities.Add(entity.Object.Entity.Name, entity);
            await AddGroupAsync(missingSettings, entityType, entity);
            AddPropertiesFromType(entity, entityType);
        }

        internal async Task AddGroupAsync(MissingEntitySettings missingSettings, Type entityType, Missing<EntityWithMissingProperties> missingEntity)
        {
            var groupName = entityType.GetAttributePropertyValue<EntitySettingsAttribute, string>("Group", DefaultGroup);
            if (string.IsNullOrWhiteSpace(DefaultGroup))
                groupName = DefaultGroup;
            var existingGroups = await _EntitySettingsGroupCache.ProvideAsync();
            if (existingGroups.TryGetValue(groupName, out EntityGroup group))
            {
                missingEntity.Object.Entity.EntityGroupId = group.Id;
                missingEntity.Object.EntityGroup = group;
            }
            else
            {
                var entityGroup = new EntityGroup { Name = groupName };
                missingEntity.Object.EntityGroup = entityGroup;
                var missingEntityGroup = new Missing<EntityGroup>(entityGroup) { IsMissing = true };
                missingSettings.EntityGroups.AddIfNew(entityGroup.Name, missingEntityGroup);
            }
        }

        internal void AddPropertiesFromType(EntityWithMissingProperties entity, Type entityType)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            var propInfos = entityType.GetProperties();
            foreach (var propInfo in propInfos)
                AddFromPropertyInfo(entity, propInfo);
        }

        internal void AddFromPropertyInfo(EntityWithMissingProperties entityWithMissingProperties, PropertyInfo propInfo)
        {
            if (entityWithMissingProperties == null) throw new ArgumentNullException(nameof(entityWithMissingProperties));
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));
            var prop = propInfo.ToEntityProperty(entityWithMissingProperties.Entity.Id, entityWithMissingProperties.SearchableProperties);
            var missingProp = new Missing<IEntityProperty>(prop) { IsMissing = true };
            entityWithMissingProperties.EntityProperties.Add(prop.Name, missingProp);
        }
    }
}