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

        public static string GetMappingEntity1Pluralized(this Type t, IDictionaryDefaultValueProvider<string, string> pluralizationDictionary = null)
        {
            if (pluralizationDictionary == null)
                pluralizationDictionary = PluralizationDictionary.Instance;
            var attribute = t.GetCustomAttributes(true)
                         .FirstOrDefault(a => (typeof(MappingEntityAttribute).IsAssignableFrom(a.GetType()))) as MappingEntityAttribute;
            return pluralizationDictionary.GetValueOrDefault(attribute?.Entity1);
        }

        public static string GetMappingEntity2Pluralized(this Type t, IDictionaryDefaultValueProvider<string, string> pluralizationDictionary = null)
        {
            if (pluralizationDictionary == null)
                pluralizationDictionary = PluralizationDictionary.Instance;
            var attribute = t.GetCustomAttributes(true)
                             .FirstOrDefault(a => (typeof(MappingEntityAttribute).IsAssignableFrom(a.GetType()))) as MappingEntityAttribute;
            return pluralizationDictionary.GetValueOrDefault(attribute?.Entity2);
        }


        public static string GetMappingEntity1Property(this Type t)
        {
            var attribute = t.GetCustomAttributes(true)
                         .FirstOrDefault(a => (typeof(MappingEntityAttribute).IsAssignableFrom(a.GetType()))) as MappingEntityAttribute;
            return attribute?.Entity1MappingProperty;
        }

        public static string GetMappingEntity2Property(this Type t)
        {
            var attribute = t.GetCustomAttributes(true)
                             .FirstOrDefault(a => (typeof(MappingEntityAttribute).IsAssignableFrom(a.GetType()))) as MappingEntityAttribute;
            return attribute?.Entity2MappingProperty;
        }
    }
}
