using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    public static class ConcreteConverter
    {
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
            var props = from prop in typeof(Tinterface).GetProperties()
                        where prop.CanRead && prop.CanWrite
                        select prop;
            var concrete = new T();
            foreach (var prop in props)
            {
                prop.SetValue(concrete, prop.GetValue(source, null), null);
            }
            return concrete;
        }        
    }
}
