using Rhyous.WebFramework.Interfaces;
using System;
using System.Linq;

namespace Rhyous.WebFramework.Services
{
    public static class TypeExtensions
    {
        public static string GetAlternateIdProperty(this Type t)
        {
            var altIdAttribute = t.GetCustomAttributes(true)
                                  .FirstOrDefault(a => (typeof(AlternateIdAttribute).IsAssignableFrom(a.GetType())))
                                  as AlternateIdAttribute;            
            return altIdAttribute?.IdProperty;
        }
    }
}
