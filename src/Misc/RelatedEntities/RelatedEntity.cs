using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.RelatedEntities
{
    public class RelatedEntity
    {
        /// <summary>
        /// The related entity's Id or Key.
        /// </summary>
        [DataMember(Order = 1)]
        [JsonProperty(Order = 1)]
        public string Id { get; set; }
        /// <summary>
        /// A string of Json representing the related entity.
        /// </summary>
        [DataMember(Order = 2)]
        [JsonProperty(Order = 2)]
        public JRaw Json { get; set; }
    }
}
