using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// Extension mehtods for Type to aid in more easily getting attributes.
    /// </summary>
    public static class MemberInfoAttributeExtensions
    {
        /// <summary>
        /// Gets the first attribute of Type t, even if that attribute is a child.
        /// This allows for inheritance both of the type and the attribute type.
        /// It also returns the exact desired type, instead of an object boxed type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="mi">An object, such as a Type, PropInfo, or MethodInfo, to which an attribute may be applied.</param>
        /// <param name="typeInheritance">Whether to look for the attribute on the object type's ancestors. True by default.</param>
        /// <param name="attributeInheritance">Whether to look for the attribute by checking the attributes ancestors. True by default.</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this MemberInfo mi, bool typeInheritance = true, bool attributeInheritance = true)
            where T : Attribute
        {
            return attributeInheritance
                ? mi.GetCustomAttributes(typeof(T), typeInheritance).FirstOrDefault() as T
                : mi.GetCustomAttributes(typeof(T), typeInheritance).FirstOrDefault(a => a.GetType() == typeof(T)) as T;
        }

        /// <summary>
        /// Gets all attributes of Type t, even if that attribute is a child.
        /// This allows for inheritance both of the type and the attribute type.
        /// It also returns the exact desired type array T[], instead of a boxed object[] type.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="mi">An object, such as a Type, PropInfo, or MethodInfo, to which an attribute may be applied.</param>
        /// <param name="typeInheritance">Whether to look for the attribute on the object type's ancestors. True by default.</param>
        /// <param name="attributeInheritance">Whether to look for the attribute by checking the attributes ancestors. True by default.</param>
        /// <returns>Attribute of type T[], not as object[].</returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo mi, bool typeInheritance = true, bool attributeInheritance = true)
            where T : Attribute
        {
            return attributeInheritance
                 ? mi.GetCustomAttributes(typeof(T), typeInheritance) as T[]
                 : mi.GetCustomAttributes(typeof(T), typeInheritance)?.Where(a => a.GetType() == typeof(T)).Select(a => a as T);
        }

        /// <summary>
        /// A method to get the value of a property of an Attribute
        /// </summary>
        /// <typeparam name="TAttribute">The attribute</typeparam>
        /// <typeparam name="TResult">The return type. It must match the property type.</typeparam>
        /// <param name="type">The type that might be decorated with the attribute.</param>
        /// <param name="prop">The attribute property name.</param>
        /// <param name="defaultValue"></param>
        /// <param name="typeInheritance">Whether to look for the attribute on the object type's ancestors. True by default.</param>
        /// <param name="attributeInheritance">Whether to look for the attribute by checking the attributes ancestors. True by default.</param>
        /// <returns>The value of an attribute's property.</returns>
        public static TResult GetAttributePropertyValue<TAttribute, TResult>(this MemberInfo type, string prop, TResult defaultValue, bool typeInheritance = true, bool attributeInheritance = true)
            where TAttribute : Attribute
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(prop)) throw new ArgumentNullException(nameof(prop));
            var attribute = type.GetAttribute<TAttribute>(typeInheritance, attributeInheritance);
            if (attribute == null)
                return defaultValue;
            var propInfo = typeof(TAttribute).GetProperty(prop);
            if (propInfo == null) throw new ArgumentNullException(nameof(prop), $"The value for {nameof(prop)} must be a valid property of {typeof(TAttribute).FullName}");
            if (propInfo.PropertyType != typeof(TResult)) throw new ArgumentException(nameof(TResult), $"The type for {nameof(TResult)} must match the type of {typeof(TAttribute).FullName}.{prop}");
            try { return (TResult)propInfo.GetValue(attribute); }
            catch { return defaultValue; }
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
        /// <param name="typeInheritance">Whether to look for the attribute on the object type's ancestors. True by default.</param>
        /// <param name="attributeInheritance">Whether to look for the attribute by checking the attributes ancestors. True by default.</param>
        /// <returns>The value of an attribute's property.</returns>
        public static TResult GetAttributePropertyValue<TResult>(this MemberInfo mi, IEnumerable<Type> attributeTypes, string prop, TResult defaultValue, bool typeInheritance = true, bool attributeInheritance = true)
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
                try { result = (TResult)attrib.GetType().GetProperty(prop)?.GetValue(attrib, null); }
                catch { continue; }
                if (!EqualityComparer<TResult>.Default.Equals(result, defaultValue))
                    return result;
            }
            return result;
        }
    }
}