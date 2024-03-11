using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class PropInfoExtensions
    {
        public static EntityProperty ToEntityProperty(this PropertyInfo propInfo, int entityId, HashSet<string> searchableProps)
        {
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));
            var propertyType = propInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                propertyType = propertyType.GetGenericArguments()[0];
            var entityPropertyAttribute = propertyType.GetAttribute<EntityPropertyAttribute>();
            var property = new EntityProperty
            {
                Name = propInfo.Name,
                EntityId = entityId,
                Type = propertyType.FullName,
                Description = propInfo.GetAttributePropertyValue(_PropertyDiscriptionAttributes, "Description", ""),
                Order = propInfo.GetAttributePropertyValue(_OrderAttributes, "Order", int.MaxValue),
                Nullable = propInfo.IsNullable(),
                Searchable = (searchableProps?.Contains(propInfo.Name) ?? false) || (entityPropertyAttribute?.Searchable ?? false)
            };
            return property;
        }

        private static readonly Type[] _PropertyDiscriptionAttributes = new[]
        {
            typeof(EntityPropertyAttribute),
            typeof(DisplayAttribute)
        };

        private static readonly Type[] _OrderAttributes = new[]
        {
            typeof(EntityPropertyAttribute)
            /// typeof(DisplayAttribute) - This throws an exception if Order is not set we are discontinuing support. Use EntityPropertyAttribute instead.
        };
    }
}
