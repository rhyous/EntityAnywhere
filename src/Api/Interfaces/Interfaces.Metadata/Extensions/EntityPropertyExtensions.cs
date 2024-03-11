using Rhyous.EntityAnywhere.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    internal static class EntityPropertyExtensions
    {
        public static void ToEntityPropertyDictionary(this IEnumerable<EntityProperty> odataProps, IEntityPropertyDictionary dict)

        {
            if (odataProps != null && odataProps.Any())
            {
                foreach (var odataProp in odataProps)
                {
                    dict.TryAdd(odataProp.Name, odataProp);
                }
            }
        }
    }
}