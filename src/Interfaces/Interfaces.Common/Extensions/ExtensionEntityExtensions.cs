using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class ExtensionEntityExtensions
    {
        public static T Get<T, TInterface>(this IEnumerable<TInterface> extensionEntity, string entity, string entityId, string property, T defaultValue)
        where TInterface : IExtensionEntity
        {
            var value = extensionEntity?.FirstOrDefault(a => a.Entity == entity && a.EntityId == entityId && a.Property.Equals(property, StringComparison.OrdinalIgnoreCase))?.Value;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (string.IsNullOrWhiteSpace(value) || !converter.IsValid(value))
            {
                return defaultValue;
            }
            return (T)(converter.ConvertFromInvariantString(value));
        }
    }
}