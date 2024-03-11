using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Services
{
    public class EntityWithMissingProperties
    {
        public Entity Entity { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public IEntityGroup EntityGroup { get; set; }

        public IDictionary<string, Missing<IEntityProperty>> EntityProperties
        {
            get { return _EntityProperties ?? (_EntityProperties = new SortedDictionary<string, Missing<IEntityProperty>>(PreferentialPropertyComparer.Instance)); }
            set { _EntityProperties = value; }
        } IDictionary<string, Missing<IEntityProperty>> _EntityProperties;

        public HashSet<string> SearchableProperties { get; } = new HashSet<string> { nameof(IId<object>.Id), nameof(IName.Name) };
    }
}
