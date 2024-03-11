using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebServices
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Get's the properties decorated with the Attribute of type TAttribute.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties<TAttribute>(this Type entityType)
            where TAttribute : Attribute
        {
            return entityType.GetProperties().Where(p =>
            {
                var attribs = p.GetCustomAttributes<TAttribute>(true);
                return attribs != null && attribs.Any();
            });
        }

        /// <summary>
        /// A method to check if a property of a type has the IgnoreTrim attribute
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="property">The property.</param>
        /// <returns>True if the property is decorated with the IgnoreTrimAttribute, false otherwise.</returns>
        public static bool IgnoreTrim(this PropertyInfo pi)
        {
            if (pi == null) throw new ArgumentNullException(nameof(pi));
            var attribute = pi.GetCustomAttribute<IgnoreTrimAttribute>(true);
            return attribute != null;
        }
    }
}