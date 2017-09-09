using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// Represents the schema of an entity property.
    /// </summary>
    [DataContract(Name = "Property")]
    public class CsdlProperty
    {
        /// <summary>
        /// Property name.
        /// </summary>
        [DataMember(Name = "Name", Order=0)]
        public string Name { get; set; }

        /// <summary>
        /// JSON Schema types.
        /// </summary>
        [DataMember(Name = "Type", Order=1)]
        public List<string> CsdlType { get; set; }

        /// <summary>
        /// JSON Schema formats.
        /// </summary>
        [DataMember(Name = "Format", Order=2)]
        public string CsdlFormat { get; set; }
    }
}