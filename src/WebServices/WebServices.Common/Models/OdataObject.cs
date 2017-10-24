using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    [DataContract]
    public class OdataObject<TEntity, TId> : IId<TId>
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
        public TId Id { get; set; }

        /// <summary>
        /// The entity instance.
        /// </summary>
        [DataMember(Order = 2)]
        public TEntity @Object
        {
            get { return _Object; }
            set
            {
                _Object = value;
                if (value is IId<TId> obj)
                    Id = obj.Id;
            }
        } private TEntity _Object;

        /// <summary>
        /// Any addenda for the entity.
        /// </summary>
        [DataMember(Order = 3)]
        public List<Addendum> Addenda
        {
            get { return _Addenda ?? (_Addenda = new List<Addendum>()); }
            set { _Addenda = value; }
        } private List<Addendum> _Addenda;

        /// <summary>
        /// Any related entity for the entity.
        /// </summary>
        [DataMember(Order = 4)]
        public List<RelatedEntityCollection> RelatedEntities
        {
            get { return _RelatedEntities ?? (_RelatedEntities = new List<RelatedEntityCollection>()); }
            set { _RelatedEntities = value; }
        } private List<RelatedEntityCollection> _RelatedEntities;

        /// <summary>
        /// A list of uris that can manage each entity property individually.
        /// </summary>
        [DataMember(Order = 5)]
        public List<ODataUri> PropertyUris { get; set; }
    }
}