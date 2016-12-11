using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    using IEntity = IPropertyCollection;

    public static class IPropertyCollectionExtensions
    {
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<IEntity> items)
            where T : class, IEntity, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        public static T ToConcrete<T>(this IEntity item)
            where T : class, IEntity, new()
        {
            return ConcreteConverter.ToConcrete<T, IEntity>(item);
        }
    }
}
