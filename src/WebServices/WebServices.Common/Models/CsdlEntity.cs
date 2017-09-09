using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object contains schema information for an entity to be returned as csdl.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class CsdlEntity<TEntity>
    {
        /// <summary>
        /// JSON Schema object of type object.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; } = "object";

        /// <summary>
        /// Additional JSON Schema object keyword.
        /// </summary>
        [DataMember(Name = "mediaEntity")]
        public bool MediaEntity { get; set; }

        /// <summary>
        /// Additional JSON Schema object keyword.
        /// </summary>
        public List<string> Keys
        {
            get { return _Keys ?? (_Keys = new List<string>()); }
            set { _Keys = value; }
        }
        private List<string> _Keys;

        /// <summary>
        /// Schema info of allowed values of properties.
        /// </summary>
        public List<CsdlProperty> Properties
        {
            get { return _Properties ?? (_Properties = new List<CsdlProperty>()); }
            set { _Properties = value; }
        }
        private List<CsdlProperty> _Properties;
    }
}
