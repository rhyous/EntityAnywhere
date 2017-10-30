using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object is used to return any entity and provide data about that entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    [DataContract]
    public class OdataObject<TEntity, TId> : Odata.OdataObject<TEntity, TId>, IId<TId>
    {
        /// <summary>
        /// Any addenda for the entity.
        /// </summary>
        [DataMember(Order = 3)]
        public List<Addendum> Addenda
        {
            get { return _Addenda ?? (_Addenda = new List<Addendum>()); }
            set { _Addenda = value; }
        } private List<Addendum> _Addenda;
    }
}