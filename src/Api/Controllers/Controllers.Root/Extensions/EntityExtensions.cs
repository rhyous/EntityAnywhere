using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    internal static class EntityExtensions
    {
        public static EntitySettingsDictionaryDto ToDto(this IEntitySettingsDictionary dict)
        {
            var dictDto = new EntitySettingsDictionaryDto();
            foreach (var kvp in dict)
            {
                dictDto.TryAdd(kvp.Key, kvp.Value.ToDto());
            }
            return dictDto;
        }

        public static EntitySettingsDto ToDto(this EntitySettings entitySettings)
        {
            var dto = entitySettings.Entity.ConcreteCopy<EntitySettingsDto, IEntity>();
            dto.SortByProperty = entitySettings.SortByProperty;
            dto.EntityGroup = entitySettings.EntityGroup.Name;
            dto.EntityProperties = entitySettings.EntityProperties as EntityPropertyDictionary;
            return dto;
        }
    }
}