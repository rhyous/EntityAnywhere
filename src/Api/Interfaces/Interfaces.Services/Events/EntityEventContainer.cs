using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This class is a container for all support entity events.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class EntityEventContainer<TEntity, TId>
        where TEntity : IId<TId>
    {
        #region Constructor
        public EntityEventContainer(
               // Maintain this order
               IEntityEventBeforeDelete<TEntity, TId> entityEventBeforeDelete,
               IEntityEventAfterDelete<TEntity, TId> entityEventAfterDelete,
               IEntityEventBeforeDeleteMany<TEntity, TId> entityEventBeforeDeleteMany,
               IEntityEventAfterDeleteMany<TEntity, TId> entityEventAfterDeleteMany,
               IEntityEventBeforePatch<TEntity, TId> entityEventBeforePatch,
               IEntityEventAfterPatch<TEntity, TId> entityEventAfterPatch,
               IEntityEventBeforePatchMany<TEntity, TId> entityEventBeforePatchMany,
               IEntityEventAfterPatchMany<TEntity, TId> entityEventAfterPatchMany,
               IEntityEventBeforePost<TEntity, TId> entityEventBeforePost,
               IEntityEventAfterPost<TEntity, TId> entityEventAfterPost,
               IEntityEventBeforePut<TEntity, TId> entityEventBeforePut,
               IEntityEventAfterPut<TEntity, TId> entityEventAfterPut,
               IEntityEventBeforeUpdateProperty<TEntity, TId> entityEventBeforeUpdateProperty,
               IEntityEventAfterUpdateProperty<TEntity, TId> entityEventAfterUpdateProperty
            )
        {
            EntityEventBeforeDelete = entityEventBeforeDelete;
            EntityEventAfterDelete = entityEventAfterDelete;
            EntityEventBeforeDeleteMany = entityEventBeforeDeleteMany;
            EntityEventAfterDeleteMany = entityEventAfterDeleteMany;
            EntityEventBeforePatch = entityEventBeforePatch;
            EntityEventAfterPatch = entityEventAfterPatch;
            EntityEventBeforePatchMany = entityEventBeforePatchMany;
            EntityEventAfterPatchMany = entityEventAfterPatchMany;
            EntityEventBeforePost = entityEventBeforePost;
            EntityEventAfterPost = entityEventAfterPost;
            EntityEventBeforePut = entityEventBeforePut;
            EntityEventAfterPut = entityEventAfterPut;
            EntityEventBeforeUpdateProperty = entityEventBeforeUpdateProperty;
            EntityEventAfterUpdateProperty = entityEventAfterUpdateProperty;
        }

        public EntityEventContainer(
            Dictionary<Type, IEntityEvent<TEntity, TId>> dict,
            EntityEventTypes<TEntity, TId> entityEventTypes) : this(
            dict[entityEventTypes.Types[0]] as IEntityEventBeforeDelete<TEntity, TId>,
            dict[entityEventTypes.Types[1]] as IEntityEventAfterDelete<TEntity, TId>,
            dict[entityEventTypes.Types[2]] as IEntityEventBeforeDeleteMany<TEntity, TId>,
            dict[entityEventTypes.Types[3]] as IEntityEventAfterDeleteMany<TEntity, TId>,
            dict[entityEventTypes.Types[4]] as IEntityEventBeforePatch<TEntity, TId>,
            dict[entityEventTypes.Types[5]] as IEntityEventAfterPatch<TEntity, TId>,
            dict[entityEventTypes.Types[6]] as IEntityEventBeforePatchMany<TEntity, TId>,
            dict[entityEventTypes.Types[7]] as IEntityEventAfterPatchMany<TEntity, TId>,
            dict[entityEventTypes.Types[8]] as IEntityEventBeforePost<TEntity, TId>,
            dict[entityEventTypes.Types[9]] as IEntityEventAfterPost<TEntity, TId>,
            dict[entityEventTypes.Types[10]] as IEntityEventBeforePut<TEntity, TId>,
            dict[entityEventTypes.Types[11]] as IEntityEventAfterPut<TEntity, TId>,
            dict[entityEventTypes.Types[12]] as IEntityEventBeforeUpdateProperty<TEntity, TId>,
            dict[entityEventTypes.Types[13]] as IEntityEventAfterUpdateProperty<TEntity, TId>)
        { 
        }
        #endregion

        public IEntityEventBeforeDelete<TEntity, TId> EntityEventBeforeDelete { get; }
        public IEntityEventAfterDelete<TEntity, TId> EntityEventAfterDelete { get; }
        public IEntityEventBeforeDeleteMany<TEntity, TId> EntityEventBeforeDeleteMany { get; }
        public IEntityEventAfterDeleteMany<TEntity, TId> EntityEventAfterDeleteMany { get; }
        public IEntityEventBeforePatch<TEntity, TId> EntityEventBeforePatch { get; }
        public IEntityEventAfterPatch<TEntity, TId> EntityEventAfterPatch { get; }
        public IEntityEventBeforePatchMany<TEntity, TId> EntityEventBeforePatchMany { get; }
        public IEntityEventAfterPatchMany<TEntity, TId> EntityEventAfterPatchMany { get; }
        public IEntityEventBeforePost<TEntity, TId> EntityEventBeforePost { get; }
        public IEntityEventAfterPost<TEntity, TId> EntityEventAfterPost { get; }
        public IEntityEventBeforePut<TEntity, TId> EntityEventBeforePut { get; }
        public IEntityEventAfterPut<TEntity, TId> EntityEventAfterPut { get; }
        public IEntityEventBeforeUpdateProperty<TEntity, TId> EntityEventBeforeUpdateProperty { get; }
        public IEntityEventAfterUpdateProperty<TEntity, TId> EntityEventAfterUpdateProperty { get; }        
    }
}