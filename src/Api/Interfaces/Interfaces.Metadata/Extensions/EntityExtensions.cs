using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    internal static class EntityExtensions
    {
        public static EntitySettings ToEntitySettings(this OdataObject<Entity, int> odataEntity)
        {
            var settings = new EntitySettings { Entity = odataEntity.Object };
            settings.EntityGroup = odataEntity.GetRelatedEntityCollection<EntityGroup, int>()?
                                              .FirstOrDefault()?.Object;
            var entityProperties = odataEntity.GetRelatedEntityCollection<EntityProperty, int>()?.Select(o => o.Object);
            entityProperties.ToEntityPropertyDictionary(settings.EntityProperties);
            settings.SortByProperty = settings.Entity.SortByPropertyId < 1
                                    ? "Id" 
                                    : entityProperties.FirstOrDefault(p => p.Id == settings.Entity.SortByPropertyId).Name;
            return settings;
        }
    }
}