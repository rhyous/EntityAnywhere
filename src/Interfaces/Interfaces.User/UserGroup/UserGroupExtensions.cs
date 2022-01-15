using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    using EntityInterface = IUserGroup;

    public static class UserGroupExtensions
    {
        /// <summary>
        /// Takes a concrete class acting as the EntityInterface and converts it to act as type T.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of EntityInterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <param name="items">An IEnumerable{EntityInterface}.</param>
        /// <returns>Returns a new IEnumerable{T}. The items are the same if the source concrete class is the same as the destination type T. If the destination type is different, then a shallow copy is returned where all public properties of EntityInterface, including inherited properties, are copied.</returns>
        public static IEnumerable<T> ToConcrete<T>(this IEnumerable<EntityInterface> items)
            where T : class, EntityInterface, new()
        {
            return items.Select(i => i.ToConcrete<T>()).ToList();
        }

        /// <summary>
        /// Takes a concrete class acting as the EntityInterface and converts it to act as type T.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of Tinterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <param name="item"></param>
        /// <returns>The same instance passed in, if the source concrete class is the same as the destination type T. If the destination type is different, then a shallow copy is returned where all public properties of EntityInterface, including inherited properties, are copied.</returns>
        public static T ToConcrete<T>(this EntityInterface item)
            where T : class, EntityInterface, new()
        {
            return ConcreteConverter.ToConcrete<T, EntityInterface>(item);
        }
    }
}
