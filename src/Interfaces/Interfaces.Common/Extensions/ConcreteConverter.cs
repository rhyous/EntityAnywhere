using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class ConcreteConverter
    {
        /// <summary>
        /// Takes a IEnumerable{Tinterface} of concrete classes each acting as the interface Tinterface and returns an IEnumerable<T>.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of Tinterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <typeparam name="TInterface">The interface both the source and destination types must implement.</typeparam>
        /// <param name="source">IEnumerable{Tinterface} of concrete classes that implement Tinterface</param>
        /// <returns>An IEnumerable{T}, where for each concrete instance, all the Tinterface properties, including inherited properties, are copied to the new instance of T. This is not a deep copy. If the type of the source concrete class and destination are the same, then the original object is returned and no copy occurrs.</returns>
        public static IEnumerable<T> ToConcrete<T, TInterface>(this IEnumerable<TInterface> items)
            where T : class, TInterface, new()
        {
            return items.Select(i => i.ToConcrete<T,TInterface>()).ToList();
        }

        /// <summary>
        /// Takes a concrete class T acting as the interface Tinterface and returns a concrete class that implements the same interface.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of Tinterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <typeparam name="Tinterface">The interface both the source and destination types must implement.</typeparam>
        /// <param name="source">The original concrete instance of Tinterface</param>
        /// <returns>An instance of T, where all the Tinterface properties, including inherited properties, are copied to the new instance of T. This is not a deep copy. This is not a deep copy. If the type of the source concrete class and destination are the same, then the original object is returned and no copy occurrs.</returns>
        public static T ToConcrete<T, Tinterface>(this Tinterface item)
            where T : class, Tinterface, new()
        {
            if (item == null)
                return default(T);
            if (typeof(T).IsAssignableFrom(item.GetType()))
                return item as T;
            var concreteItem = new T();
            return item.ConcreteCopy<T, Tinterface>();
        }

        /// <summary>
        /// Takes a concrete class T acting as the interface Tinterface and returns another concrete class that implements the same interface.
        /// </summary>
        /// <typeparam name="T">The destination concrete type of Tinterface. It must be a class. It must implement Tinterface. It must have a generic constructor.</typeparam>
        /// <typeparam name="Tinterface">The interface both the source and destination types must implement.</typeparam>
        /// <param name="source">The original concrete instance of Tinterface</param>
        /// <returns>An instance of T, where all the Tinterface properties, including inherited properties, are copied to the instance of T. This is not a deep copy. A null source results in null.</returns>
        public static T ConcreteCopy<T, Tinterface>(this Tinterface source)
            where T : class, Tinterface, new()
        {
            if (source == null)
                return null;
            var itype = typeof(Tinterface);
            var props = (new Type[] { itype }).Concat(itype.GetInterfaces())
                                              .SelectMany(i => i.GetProperties());
            var concrete = new T();
            foreach (var prop in props)
            {
                prop.SetValue(concrete, prop.GetValue(source, null), null);
            }
            return concrete;
        }
    }
}
