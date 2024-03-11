using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rhyous.EntityAnywhere.Attributes
{
    internal class DynamicAttributePropertyReader
    {
        private readonly ITypeInfoResolver _TypeInfoResolver;

        public DynamicAttributePropertyReader(ITypeInfoResolver typeInfoResolver)
        {
            _TypeInfoResolver = typeInfoResolver;
        }
        /// <summary>
        /// A method to get the value of a property from any of the provided attributes.
        /// Sometimes multiple attributes must be respected, such as DataMember or JsonProperty.
        /// </summary>
        /// <typeparam name="TResult">The return type. It must match the property type.</typeparam>
        /// <param name="mi">The type that might be decorated with the attribute.</param>
        /// <param name="attributeTypes">The attribute types.</param>
        /// <param name="prop">The attribute property name.</param>
        /// <param name="defaultValue">The default value if no attribute is applied.</param>
        /// <returns>The value of an attribute's property.</returns>
        public TResult GetPropertyValue<TResult>(MemberInfo mi, IEnumerable<Type> attributeTypes, string prop, TResult defaultValue)
        {
            var attribs = mi.GetCustomAttributes();
            if (!attribs.Any())
                return defaultValue;
            var matchingAttribs = new List<Attribute>();
            foreach (var at in attributeTypes)
            {
                var matches = attribs.Where(a => at.IsAssignableFrom(a.GetType()));
                if (matches.Any())
                    matchingAttribs.AddRange(matches);
            }
            if (!matchingAttribs.Any())
                return defaultValue;
            TResult result = defaultValue;
            foreach (var attrib in matchingAttribs)
            {
                try
                {
                    var typeInfo = _TypeInfoResolver.Resolve(attrib.GetType());
                    if (typeInfo.Properties.TryGetValue(prop, out PropertyInfo propInfo))
                        result = (TResult)propInfo?.GetValue(attrib, null);
                }
                catch { continue; }
                if (!EqualityComparer<TResult>.Default.Equals(result, defaultValue))
                    return result;
            }
            return result;
        }
    }
}
