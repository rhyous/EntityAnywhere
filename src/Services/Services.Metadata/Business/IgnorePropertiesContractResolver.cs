using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services
{
    public class IgnorePropertiesContractResolver : DefaultContractResolver
    {
        private readonly HashSet<string> _Ignores;

        public IgnorePropertiesContractResolver()
        {
            _Ignores = new HashSet<string>();
        }

        public void Ignore(params string[] jsonPropertyNames)
        {
            foreach (var prop in jsonPropertyNames)
                _Ignores.Add(prop);
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (_Ignores.Contains(property.PropertyName))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }
            return property;
        }
    }
}