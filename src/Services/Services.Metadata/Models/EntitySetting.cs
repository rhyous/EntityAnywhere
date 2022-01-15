using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Services
{
    [ExcludeFromCodeCoverage]
    public class EntitySetting : Entity
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public string EntityGroup { get; set; }

        public IDictionary<string, IEntityProperty> EntityProperties
        {
            get { return _EntityProperties ?? (_EntityProperties = new Dictionary<string, IEntityProperty>()); }
            set { _EntityProperties = value; }
        } IDictionary<string, IEntityProperty> _EntityProperties;
    }
}