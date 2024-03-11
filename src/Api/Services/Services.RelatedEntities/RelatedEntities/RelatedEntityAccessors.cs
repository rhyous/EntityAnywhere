using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public class RelatedEntityAccessors<TEntity, TInterface, TId> : IRelatedEntityAccessors<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public RelatedEntityAccessors(
            IRelatedEntityExtensions<TEntity, TInterface, TId> relatedEntityExtensions,
            IRelatedEntityManyToOne<TEntity, TInterface, TId> relatedEntityManyToOne,
            IRelatedEntityOneToMany<TEntity, TInterface, TId> relatedEntityOneToMany,
            IRelatedEntityManyToMany<TEntity, TInterface, TId> relatedEntityManyToMany)
        {
            if (relatedEntityExtensions is null) { throw new ArgumentNullException(nameof(relatedEntityExtensions)); }
            if (relatedEntityManyToOne is null) { throw new ArgumentNullException(nameof(relatedEntityManyToOne)); }
            if (relatedEntityOneToMany is null) { throw new ArgumentNullException(nameof(relatedEntityOneToMany)); }
            if (relatedEntityManyToMany is null) { throw new ArgumentNullException(nameof(relatedEntityManyToMany)); }
            List = new List<IGetRelatedEntitiesAsync<TEntity, TInterface, TId>>
            {
                relatedEntityExtensions,
                relatedEntityManyToOne,
                relatedEntityOneToMany,
                relatedEntityManyToMany
            };
        }

        public IList<IGetRelatedEntitiesAsync<TEntity, TInterface, TId>> List { get; }
    }
}
