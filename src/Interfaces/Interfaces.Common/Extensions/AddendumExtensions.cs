using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class AddendaExtensions
    {
        public static T Get<T>(this IEnumerable<IAddendum> addenda, string entity, string entityId, string property, T defaultValue)
        {
            var value = addenda.FirstOrDefault(a=>a.Entity == entity && a.EntityId == entityId && a.Property == property)?.Value;
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (string.IsNullOrWhiteSpace(value) || !converter.IsValid(value))
            {
                return defaultValue;
            }
            return (T)(converter.ConvertFromInvariantString(value));
        }
    }
}
