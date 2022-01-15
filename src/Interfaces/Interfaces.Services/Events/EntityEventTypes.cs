using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This class holds all EntityEvent types in a list that 
    /// is ordered exactly the same as these types are ordered
    /// in the <see cref="EntityEventContainer{TEntity}"/> constructor.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The type of the entity's Id field.</typeparam>
    public class EntityEventTypes<TEntity, TId>
        where  TEntity : IId<TId>
    {
        public List<Type> Types
        {
            get
            {
                return _Types ?? (_Types = new List<Type>
                {
                    // Maintain this order
                    typeof(IEntityEventBeforeDelete<TEntity>),
                    typeof(IEntityEventAfterDelete<TEntity>),
                    typeof(IEntityEventBeforeDeleteMany<TEntity>),
                    typeof(IEntityEventAfterDeleteMany<TEntity, TId>),
                    typeof(IEntityEventBeforePatch<TEntity, TId>),
                    typeof(IEntityEventAfterPatch<TEntity, TId>),
                    typeof(IEntityEventBeforePatchMany<TEntity, TId>),
                    typeof(IEntityEventAfterPatchMany<TEntity, TId>),
                    typeof(IEntityEventBeforePost<TEntity>),
                    typeof(IEntityEventAfterPost<TEntity>),
                    typeof(IEntityEventBeforePut<TEntity>),
                    typeof(IEntityEventAfterPut<TEntity>),
                    typeof(IEntityEventBeforeUpdateProperty<TEntity>),
                    typeof(IEntityEventAfterUpdateProperty<TEntity>)
                });
            } 
        } public List<Type> _Types;
    }
}
