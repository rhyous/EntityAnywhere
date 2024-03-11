using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.StringLibrary.Pluralization;
using Rhyous.EntityAnywhere.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class TypeExtensions
    {        
        public static bool ImplementsGenericInterface(this Type t, Type genericInterfaceType)
        {
            return t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType);
        }

        public static List<Type> GetInterfaceInheritance(this Type type)
        {
            var interfaces = new List<Type>();
            foreach (Type parentInterface in type.GetInterfaces())
            {
                interfaces.Add(parentInterface);
                interfaces.AddRange(parentInterface.GetInterfaceInheritance());
            }
            return interfaces;
        }

        public static string GetMappedEntity1(this Type t)
        {
            return t.GetAttribute<MappingEntityAttribute>()?.Entity1;
        }

        public static string GetMappedEntity1Pluralized(this Type t, IDictionaryDefaultValueProvider<string, string> pluralizationDictionary = null)
        {
            var uriTemplate = t.GetAttribute<MappingEntityAttribute>()?.Entity1UriTemplate;
            if (!string.IsNullOrWhiteSpace(uriTemplate))
                return uriTemplate;
            return t.GetMappedEntity1().Pluralize();
        }
        
        /// <summary>This is </summary>
        /// <param name="t">The mapping entity type.
        /// This is a not a mapping entity, but one of the entities being mapped.</param>
        /// <returns>The name of the property as configured in the attribute.</returns>
        public static string GetMappedEntity1Property(this Type t)
        {
            return t.GetAttribute<MappingEntityAttribute>().Entity1MappingProperty;
        }

        /// <summary>This is </summary>
        /// <param name="t">The mapping entity type.
        /// This is a not a mapping entity, but one of the entities being mapped.</param>
        /// <returns>PropertyInfo</returns>
        public static PropertyInfo GetMappedEntity1PropertyInfo(this Type t)
        {
            var entity1TypeName = t.GetMappedEntity1Property();
            var e1Property = t.GetProperties().FirstOrDefault(p => p.Name == entity1TypeName);
            return e1Property;
        }

        public static string GetMappedEntity2(this Type t, IDictionaryDefaultValueProvider<string, string> pluralizationDictionary = null)
        {
            return t.GetAttribute<MappingEntityAttribute>()?.Entity2;
        }

        public static string GetMappedEntity2Pluralized(this Type t, IDictionaryDefaultValueProvider<string, string> pluralizationDictionary = null)
        {
            var uriTemplate = t.GetAttribute<MappingEntityAttribute>()?.Entity2UriTemplate;
            if (!string.IsNullOrWhiteSpace(uriTemplate))
                return uriTemplate;
            return t.GetMappedEntity2().Pluralize();
        }

        public static string GetMappedEntity2Property(this Type t)
        {
            return t.GetAttribute<MappingEntityAttribute>().Entity2MappingProperty;
        }

        /// <summary>This is </summary>
        /// <param name="t">The mapping entity type.
        /// This is a not a mapping entity, but one of the entities being mapped.</param>
        /// <returns>PropertyInfo</returns>
        public static PropertyInfo GetMappedEntity2PropertyInfo(this Type t)
        {

            var entity2TypeName = t.GetMappedEntity2Property();
            var e2Property = t.GetProperties().FirstOrDefault(p => p.Name == entity2TypeName);
            return e2Property;
        }

        /// <summary>
        /// A quick method to get the AlternateKey of an Entity.
        /// </summary>
        /// <param name="t">The type that is decorated with the AlternateKey attribute.</param>
        /// <returns>The name of the AlternateKey property.</returns>
        public static string GetAlternateKeyProperty(this Type t)
        {
            var attribute = t?.GetCustomAttributes(typeof(AlternateKeyAttribute), true).FirstOrDefault() as AlternateKeyAttribute;
            return attribute?.KeyProperty;
        }

        /// <summary>
        /// A quick method to get the list of editable properties of an entity.
        /// </summary>
        /// <param name="t">The type of entity to scan for editable properties.</param>
        /// <returns>The list of editable properties.</returns>
        public static IEnumerable<PropertyInfo> GetEditableProperties(this Type t)
        {
           return t?.GetProperties().Where(prop => !Attribute.IsDefined(prop, typeof(EditableAttribute)) ||
                                                    prop.GetCustomAttribute<EditableAttribute>().AllowEdit);
        }

        /// <summary>
        /// A quick method to get the AdditionalTypes of an Entity.
        /// </summary>
        /// <typeparam name="T">The type of the concrete attribute that implements IAdditionalTypes.</typeparam>
        /// <param name="t">The type that is decorated with a concrete attribute that implements IAdditionalTypes interface.</param>
        /// <returns>The list of AdditionalTypes.</returns>
        public static Type[] GetAdditionalTypes<T>(this Type t)
            where T: IAdditionalTypes
        {
            var additionalTypesAttrib = t.GetCustomAttributes(typeof(T), true).FirstOrDefault() as IAdditionalTypes;
            if (additionalTypesAttrib != null && additionalTypesAttrib.AdditionalTypes != null && additionalTypesAttrib.AdditionalTypes.Any())
                return additionalTypesAttrib?.AdditionalTypes;
            var alternateKeyAttribute = t.GetAttribute<AlternateKeyAttribute>();
            if (alternateKeyAttribute != null)
                return new[] { t.GetPropertyInfo(alternateKeyAttribute.KeyProperty).PropertyType };
            return null;
        }
    }
}