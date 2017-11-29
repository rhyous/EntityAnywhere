using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Behaviors
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

        public static T GetAttribute<T>(this Type t)
            where T : Attribute
        {
            return t.GetCustomAttributes(true)?.FirstOrDefault(a => (typeof(T).IsAssignableFrom(a.GetType()))) as T;
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
            return (pluralizationDictionary ?? PluralizationDictionary.Instance).GetValueOrDefault(t.GetMappedEntity1());
        }
        
        public static string GetMappedEntity1Property(this Type t)
        {
            return t.GetAttribute<MappingEntityAttribute>().Entity1MappingProperty;
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
            return (pluralizationDictionary ?? PluralizationDictionary.Instance).GetValueOrDefault(t.GetMappedEntity2());
        }

        public static string GetMappedEntity2Property(this Type t)
        {
            return t.GetAttribute<MappingEntityAttribute>().Entity2MappingProperty;
        }
    }
}
