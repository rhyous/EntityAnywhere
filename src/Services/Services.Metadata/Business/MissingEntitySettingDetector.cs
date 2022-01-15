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

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// This class determines whether each Entity, EntityProperty, or EntityGoup already exists or not.
    /// </summary>
    public class MissingEntitySettingDetector : IMissingEntitySettingDetector
    {
        private const string DefaultGroup = "Miscellaneous";

        private readonly Type[] _EnitityDiscriptionAttributes = new[] { typeof(EntitySettingsAttribute), typeof(DescriptionAttribute) };
        private IDictionary<string, EntityGroup> _ExistingGroups;

        public MissingEntitySettingDetector(IDictionary<string, EntityGroup> existingGroups)
        {
            _ExistingGroups = existingGroups;
        }

        public MissingEntitySettings Detect(IDictionary<string, EntitySetting> settings, IEnumerable<Type> entityTypes)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (entityTypes == null) throw new ArgumentNullException(nameof(entityTypes));
            var missingSettings = new MissingEntitySettings();
            foreach (var entityType in entityTypes)
            {
                if (!settings.TryGetValue(entityType.Name, out EntitySetting entity))
                {
                    AddFromType(missingSettings, entityType);
                    continue;
                }
                var entity2 = entity.ConcreteCopy<Entity2, IEntity>();
                var missing = new Missing<Entity2>(entity2);
                foreach (var propInfo in entityType.GetProperties())
                {
                    // If property should be excluded from Metadata, don't include it.
                    if (propInfo.ExcludeFromMetadata())
                        continue;
                    if (!entity.EntityProperties.TryGetValue(propInfo.Name, out IEntityProperty prop))
                    {
                        missingSettings.Entities.AddIfNew(entity.Name, missing);
                        AddFromPropertyInfo(missing, propInfo);
                    }
                }
            }
            return missingSettings;
        }

        internal void AddFromType(MissingEntitySettings missingSettings, Type entityType)
        {
            if (missingSettings == null) throw new ArgumentNullException(nameof(missingSettings));
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));
            var entity = new Missing<Entity2>
            {
                Object = new Entity2
                {
                    Name = entityType.Name,
                    Description = entityType.GetAttributePropertyValue(_EnitityDiscriptionAttributes, "Description", ""),
                    Enabled = true,
                    EntityGroupId = 1
                },
                IsMissing = true
            };
            missingSettings.Entities.Add(entity.Object.Name, entity);
            AddGroup(missingSettings, entityType, entity);
            AddPropertiesFromType(entity, entityType);
        }

        internal void AddGroup(MissingEntitySettings missingSettings, Type entityType, Missing<Entity2> entity)
        {
            var groupName = entityType.GetAttributePropertyValue<EntitySettingsAttribute, string>("Group", DefaultGroup) ?? DefaultGroup;
            if (_ExistingGroups.TryGetValue(groupName, out EntityGroup group))
            {
                entity.Object.EntityGroupId = group.Id;
                entity.Object.EntityGroup = group.Name;
            }
            else
            {
                var entityGroup = new EntityGroup { Name = groupName };
                entity.Object.EntityGroup = entityGroup.Name;
                var missingEntityGroup = new Missing<EntityGroup>(entityGroup) { IsMissing = true };
                missingSettings.EntityGroups.AddIfNew(entityGroup.Name, missingEntityGroup);
            }
        }

        internal void AddPropertiesFromType(Entity2 entity, Type entityType)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            var properties = new List<EntityProperty>();
            var propInfos = entityType.GetProperties().OrderBy(p => p.Name, new PreferentialPropertyComparer());
            foreach (var propInfo in propInfos)
                AddFromPropertyInfo(entity, propInfo);
        }

        internal void AddFromPropertyInfo(Entity2 entitySetting, PropertyInfo propInfo)
        {
            if (entitySetting == null) throw new ArgumentNullException(nameof(entitySetting));
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));
            var prop = propInfo.ToEntityProperty(entitySetting.Id);
            var missingProp = new Missing<IEntityProperty>(prop) { IsMissing = true };
            entitySetting.EntityProperties.Add(prop.Name, missingProp);
        }
    }
}