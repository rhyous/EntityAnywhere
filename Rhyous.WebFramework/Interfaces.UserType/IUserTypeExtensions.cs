using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class IUserTypeExtensions
    {       
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<IUserType> items)
            where T : class, IUserType, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        public static T ToConcrete<T>(this IUserType item)
            where T : class, IUserType, new()
        {
            return ConcreteConverter.ToConcrete<T, IUserType>(item);
        }
    }
}
