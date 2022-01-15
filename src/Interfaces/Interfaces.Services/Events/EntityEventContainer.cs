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
               IEntityEventBeforeDelete<TEntity> entityEventBeforeDelete,
               IEntityEventAfterDelete<TEntity> entityEventAfterDelete,
               IEntityEventBeforeDeleteMany<TEntity> entityEventBeforeDeleteMany,
               IEntityEventAfterDeleteMany<TEntity, TId> entityEventAfterDeleteMany,
               IEntityEventBeforePatch<TEntity, TId> entityEventBeforePatch,
               IEntityEventAfterPatch<TEntity, TId> entityEventAfterPatch,
               IEntityEventBeforePatchMany<TEntity, TId> entityEventBeforePatchMany,
               IEntityEventAfterPatchMany<TEntity, TId> entityEventAfterPatchMany,
               IEntityEventBeforePost<TEntity> entityEventBeforePost,
               IEntityEventAfterPost<TEntity> entityEventAfterPost,
               IEntityEventBeforePut<TEntity> entityEventBeforePut,
               IEntityEventAfterPut<TEntity> entityEventAfterPut,
               IEntityEventBeforeUpdateProperty<TEntity> entityEventBeforeUpdateProperty,
               IEntityEventAfterUpdateProperty<TEntity> entityEventAfterUpdateProperty
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
            Dictionary<Type, IEntityEvent<TEntity>> dict,
            EntityEventTypes<TEntity, TId> entityEventTypes) : this(
            dict[entityEventTypes.Types[0]] as IEntityEventBeforeDelete<TEntity>,
            dict[entityEventTypes.Types[1]] as IEntityEventAfterDelete<TEntity>,
            dict[entityEventTypes.Types[2]] as IEntityEventBeforeDeleteMany<TEntity>,
            dict[entityEventTypes.Types[3]] as IEntityEventAfterDeleteMany<TEntity, TId>,
            dict[entityEventTypes.Types[4]] as IEntityEventBeforePatch<TEntity, TId>,
            dict[entityEventTypes.Types[5]] as IEntityEventAfterPatch<TEntity, TId>,
            dict[entityEventTypes.Types[6]] as IEntityEventBeforePatchMany<TEntity, TId>,
            dict[entityEventTypes.Types[7]] as IEntityEventAfterPatchMany<TEntity, TId>,
            dict[entityEventTypes.Types[8]] as IEntityEventBeforePost<TEntity>,
            dict[entityEventTypes.Types[9]] as IEntityEventAfterPost<TEntity>,
            dict[entityEventTypes.Types[10]] as IEntityEventBeforePut<TEntity>,
            dict[entityEventTypes.Types[11]] as IEntityEventAfterPut<TEntity>,
            dict[entityEventTypes.Types[12]] as IEntityEventBeforeUpdateProperty<TEntity>,
            dict[entityEventTypes.Types[13]] as IEntityEventAfterUpdateProperty<TEntity>)
        { 
        }
        #endregion

        public IEntityEventBeforeDelete<TEntity> EntityEventBeforeDelete { get; }
        public IEntityEventAfterDelete<TEntity> EntityEventAfterDelete { get; }
        public IEntityEventBeforeDeleteMany<TEntity> EntityEventBeforeDeleteMany { get; }
        public IEntityEventAfterDeleteMany<TEntity, TId> EntityEventAfterDeleteMany { get; }
        public IEntityEventBeforePatch<TEntity, TId> EntityEventBeforePatch { get; }
        public IEntityEventAfterPatch<TEntity, TId> EntityEventAfterPatch { get; }
        public IEntityEventBeforePatchMany<TEntity, TId> EntityEventBeforePatchMany { get; }
        public IEntityEventAfterPatchMany<TEntity, TId> EntityEventAfterPatchMany { get; }
        public IEntityEventBeforePost<TEntity> EntityEventBeforePost { get; }
        public IEntityEventAfterPost<TEntity> EntityEventAfterPost { get; }
        public IEntityEventBeforePut<TEntity> EntityEventBeforePut { get; }
        public IEntityEventAfterPut<TEntity> EntityEventAfterPut { get; }
        public IEntityEventBeforeUpdateProperty<TEntity> EntityEventBeforeUpdateProperty { get; }
        public IEntityEventAfterUpdateProperty<TEntity> EntityEventAfterUpdateProperty { get; }        
    }
}