using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public static class TypeExtensions
    {
        public static string GetAlternateIdProperty(this Type t)
        {
            var attribute = t.GetCustomAttributes(true)
                                  .FirstOrDefault(a => (typeof(AlternateKeyAttribute).IsAssignableFrom(a.GetType()))) as AlternateKeyAttribute;
            return attribute?.KeyProperty;
        }

        public static Type[] GetAdditionalTypes<T>(this Type t)
            where T: IAdditionalTypes
        {
            var attribute = t.GetCustomAttributes(typeof(T), true).FirstOrDefault() as IAdditionalTypes;
            return attribute?.Types;
        }
    }
}
