using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class ConcreteConverter
    {
        public static IEnumerable<T> ToConcrete<T, Tinterface>(this IEnumerable<Tinterface> items)
            where T : class, Tinterface, new()
        {
            return items.Select(i => i.ToConcrete<T,Tinterface>()).ToList();
        }

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

        public static T ConcreteCopy<T, Tinterface>(this Tinterface source)
            where T : class, Tinterface, new()

        {
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
