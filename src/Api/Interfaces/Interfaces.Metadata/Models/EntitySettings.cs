using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Entities;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Rhyous.EntityAnywhere.Interfaces
{
    [ExcludeFromCodeCoverage]
    public class EntitySettings
    {
        public Entity Entity { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public EntityGroup EntityGroup { get; set; }

        public string SortByProperty { get; set; }

        public IEntityPropertyDictionary EntityProperties
        {
            get { return _EntityProperties ?? (_EntityProperties = new EntityPropertyDictionary()); }
            set { _EntityProperties = value; }
        } IEntityPropertyDictionary _EntityProperties;
    }
}