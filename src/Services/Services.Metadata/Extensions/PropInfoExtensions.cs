using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services
{
    public static class PropInfoExtensions
    {
        public static EntityProperty ToEntityProperty(this PropertyInfo propInfo, int entityId)
        {
            if (propInfo == null) throw new ArgumentNullException(nameof(propInfo));
            var propertyType = propInfo.PropertyType;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                propertyType = propertyType.GetGenericArguments()[0];
            return new EntityProperty
            {
                Name = propInfo.Name,
                EntityId = entityId,
                Type = propertyType.FullName,
                Description = propInfo.GetAttributePropertyValue(_PropertyDiscriptionAttributes, "Description", ""),
                Order = propInfo.GetAttributePropertyValue(_OrderAttributes, "Order", int.MaxValue),
                Nullable = propInfo.IsNullable()

            };
        }

        private static readonly Type[] _PropertyDiscriptionAttributes = new[]
        {
            typeof(EntityPropertyAttribute),
            typeof(DisplayAttribute)
        };

        private static readonly Type[] _OrderAttributes = new[]
        {
            typeof(EntityPropertyAttribute),
            typeof(DisplayAttribute)
        };
    }
}
