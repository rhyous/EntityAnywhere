using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Services
{
    public class Entity2 : Entity
    {
        [JsonIgnore]
        [IgnoreDataMember]
        public string EntityGroup { get; set; }

        public IDictionary<string, Missing<IEntityProperty>> EntityProperties
        {
            get { return _EntityProperties ?? (_EntityProperties = new Dictionary<string, Missing<IEntityProperty>>()); }
            set { _EntityProperties = value; }
        } IDictionary<string, Missing<IEntityProperty>> _EntityProperties;
    }
}
