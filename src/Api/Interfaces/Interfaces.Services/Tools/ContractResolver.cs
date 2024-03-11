using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Tools
{
    public sealed class ContractResolver : ExcludeEmptyEnumerablesContractResolver
    {
        #region Singleton
        private static readonly Lazy<ContractResolver> Lazy = new Lazy<ContractResolver>(() => new ContractResolver());
        new public static ContractResolver Instance { get { return Lazy.Value; } }
        internal ContractResolver() { }
        #endregion

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            // If JsonPropertyAttribute is used for ordering, use it
            if (type.GetProperties().Any(p => p.GetCustomAttribute<JsonPropertyAttribute>()?.Order > 0))
                return properties;
            // If JsonPropertyAttribute is not used, use alphabetical order, except put preferential properties at the front
            // Json order doesn't matter for functionality. However, this is a much better human readability experience.
            var orderedProperties = GetPreferentialOrder(type, properties);
            return orderedProperties;
        }

        internal static List<JsonProperty> GetPreferentialOrder(Type type, IList<JsonProperty> properties)
        {
            var prefsFromAttributes = GetPreferentialPropertiesFromAttributes(type);
            if (prefsFromAttributes != null && prefsFromAttributes.Any())
            {
                foreach (var preferredPropertyName in prefsFromAttributes)
                    PreferentialPropertyComparer.Instance.PreferredProperties.Add(preferredPropertyName);
            }
            return properties.OrderBy(p => p.PropertyName, PreferentialPropertyComparer.Instance).ToList();
        }

        internal static List<string> GetPreferentialPropertiesFromAttributes(Type type)
        {
            var list = new List<string>();
            // KeyAttribute
            var keyProp = type.GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
            if (keyProp != null)
                list.Add(keyProp.Name);
            // AlternateKeyAttribute
            var attribute = type.GetCustomAttributes(true).FirstOrDefault(a => (typeof(AlternateKeyAttribute).IsAssignableFrom(a.GetType()))) as AlternateKeyAttribute;
            if (attribute != null && !string.IsNullOrWhiteSpace(attribute.KeyProperty))
                list.Add(attribute.KeyProperty);
            return list;
        }
    }
}
