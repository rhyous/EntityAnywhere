using Rhyous.WebFramework.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    [DataContract]
    public class OdataObject<TEntity>
    {
        /// <summary>
        /// The entity's web service uri.
        /// </summary>
        [DataMember(Order = 0)]
        public Uri Uri { get; set; }

        /// <summary>
        /// The entity instance.
        /// </summary>
        [DataMember(Order = 1)]
        public TEntity @Object { get; set; }

        /// <summary>
        /// Any addenda for the entity.
        /// </summary>
        [DataMember(Order = 2)]
        public List<Addendum> Addenda { get; set; }

        /// <summary>
        /// Any related entity for the entity.
        /// </summary>
        [DataMember(Order = 2)]
        public List<string> RelatedEntities { get; set; }

        /// <summary>
        /// A list of uris that can manage each entity property individually.
        /// </summary>
        [DataMember(Order = 3)]
        public List<ODataUri> PropertyUris { get; set; }
    }
}
