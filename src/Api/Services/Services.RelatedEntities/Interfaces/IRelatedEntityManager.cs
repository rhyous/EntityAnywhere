using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public interface IRelatedEntityManager<TEntity, TInterface, TId>
        : IGetRelatedEntitiesAsync<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        List<RelatedEntityCollection> GetRelatedEntities(IEnumerable<TInterface> entities, NameValueCollection parameters);
        List<RelatedEntityCollection> GetRelatedEntities(TInterface entity, NameValueCollection parameters);
    }
}