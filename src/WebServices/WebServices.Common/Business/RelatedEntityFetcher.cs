using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    public class RelatedEntityFetcher<TEntity, TInterface, TId> : IRelatedEntityFetcher<TEntity, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public void Fetch(IEnumerable<OdataObject<TEntity, TId>> entities, NameValueCollection urlParameters)
        {
            if (entities != null && entities.Any())
            {
                var relatedEntityCollection = Service.GetRelatedEntities(entities.Select(o => o.Object), urlParameters);
                if (relatedEntityCollection != null && relatedEntityCollection.Any())
                    Collater.Collate(entities, relatedEntityCollection);
            }
        }

        internal virtual IGetRelatedEntities<TInterface> Service
        {
            get { return _Service ?? (_Service = new RelatedEntityManager<TEntity, TInterface, TId>()); }
            set { _Service = value; }
        } private IGetRelatedEntities<TInterface> _Service;

        public IRelatedEntityCollater<TEntity, TId> Collater
        {
            get { return _Collater ?? (_Collater = new RelatedEntitySorter<TEntity, TId>()); }
            set { _Collater = value; }
        } private IRelatedEntityCollater<TEntity, TId> _Collater;
    }
}
