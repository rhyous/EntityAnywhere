using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public class EntitySettingsProvider : IEntitySettingsProvider
    {
        private readonly IAdminEntityClientAsync<Entity, int> _EntityClient;

        public EntitySettingsProvider(IAdminEntityClientAsync<Entity, int> entityClient)
        {
            _EntityClient = entityClient ?? throw new ArgumentNullException(nameof(entityClient));
        }

        public async Task<IDictionary<string, EntitySetting>> GetAsync()
        {
            var settings = new Dictionary<string, EntitySetting>();
            var odataEntities = await _EntityClient.GetByQueryParametersAsync("$Expand=EntityGroup,EntityProperty");
            if (odataEntities == null || !odataEntities.Any())
                return settings; // Return empty dictionary.
            foreach (var odataEntity in odataEntities)
            {
                var entitySetting = odataEntity.Object.ToConcrete<EntitySetting>();
                entitySetting.EntityGroup = odataEntity.GetRelatedEntityCollection<EntityGroup, int>()?
                                                       .FirstOrDefault()?.Object.Name;
                var properties = odataEntity.GetRelatedEntityCollection<EntityProperty, int>()?
                                 .Select(o => new KeyValuePair<string, IEntityProperty>(o.Object.Name, o.Object));
                if (properties != null && properties.Any())
                    entitySetting.EntityProperties.AddRange(properties);
                settings.Add(entitySetting.Name, entitySetting);
            }
            return settings;
        }
    }
}