using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class IUserExtensions
    {       
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<IUser> items)
            where T : class, IUser, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        public static T ToConcrete<T>(this IUser item)
            where T : class, IUser, new()
        {
            return ConcreteConverter.ToConcrete<T, IUser>(item);
        }
    }
}
