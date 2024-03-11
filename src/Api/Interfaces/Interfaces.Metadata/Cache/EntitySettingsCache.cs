using Rhyous.Collections;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Cache
{
    public class EntitySettingsCache : CacheBase<IEntitySettingsDictionary>, IEntitySettingsCache
    {
        private readonly IAdminEntityClientAsync<Entity, int> _EntityClient;
        private readonly EntitySettings _DefaultEntitySettings = new EntitySettings { SortByProperty = "Id", Entity = new Entity { SortOrder = SortOrder.Ascending } };

        public EntitySettingsCache(IAdminEntityClientAsync<Entity, int> entityClient)
            : base(new EntitySettingsDictionary())
        {
            _EntityClient = entityClient ?? throw new ArgumentNullException(nameof(entityClient));
        }

        /// <summary>Provides the entity settings from cache, or the default EntitySettings if the cache is not there or not ready.</summary>
        /// <param name="entityType">The entity type</param>
        /// <returns>Returns the entity settings from cache, or the default EntitySettings.</returns>
        /// <remarks>We need to avoid an infinite loop. If we were to try build the cache if it doesn't exist, we would have 
        /// and infinite loop calling Entity, EntityProperty, or EntityGroup. During startup, the default should be used
        /// until the cache is populated.</remarks>
        public EntitySettings Provide(Type entityType)
        {
            if (_Cache.TryGetValue(entityType.Name, out EntitySettings entitySettings))
                return entitySettings;
            return _DefaultEntitySettings;
        }

        protected override async Task CreateCacheAsync()
        {
            var odataEntities = await _EntityClient.GetByQueryParametersAsync("$Expand=EntityGroup,EntityProperty");
            if (odataEntities == null || !odataEntities.Any())
                return;
            foreach (var odataEntity in odataEntities)
            {
                var entitySetting = odataEntity.ToEntitySettings();
                _Cache.TryAdd(entitySetting.Entity.Name, entitySetting);
            }
        }
    }
}