using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public interface IRelatedEntityAccessors<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        IList<IGetRelatedEntitiesAsync<TEntity, TInterface, TId>> List { get; }
    }
}