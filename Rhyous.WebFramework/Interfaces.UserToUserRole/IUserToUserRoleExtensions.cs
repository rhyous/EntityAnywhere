using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    using Entity = IUserToUserRole;

    public static class IUserToUserRoleExtensions
    {
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<Entity> items)
            where T : class, Entity, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        public static T ToConcrete<T>(this Entity item)
            where T : class, Entity, new()
        {
            return ConcreteConverter.ToConcrete<T, Entity>(item);
        }
    }
}
