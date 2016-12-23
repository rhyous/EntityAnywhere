using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    using EntityInterface = IUserToUserType;

    public static class IUserToUserTypeExtensions
    {
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<EntityInterface> items)
            where T : class, EntityInterface, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        public static T ToConcrete<T>(this EntityInterface item)
            where T : class, EntityInterface, new()
        {
            return ConcreteConverter.ToConcrete<T, EntityInterface>(item);
        }
    }
}
