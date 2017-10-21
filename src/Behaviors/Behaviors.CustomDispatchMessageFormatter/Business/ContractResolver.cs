using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Behaviors
{
    public class ContractResolver : DefaultContractResolver
    {
        #region Singleton
        private static readonly Lazy<ContractResolver> Lazy = new Lazy<ContractResolver>(() => new ContractResolver());
        public static ContractResolver Instance { get { return Lazy.Value; } }
        internal ContractResolver() { }
        #endregion

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            var ordered = properties.OrderBy(p=>p.PropertyName, PreferentialComparer.Instance);
            return ordered.ToList();
        }
    }
}
