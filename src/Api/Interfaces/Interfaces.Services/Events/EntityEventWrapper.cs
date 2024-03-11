using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This is a wrapper for EntityEvents. It makes sure that all events
    /// are executed via TryLog, which will log exceptions. It also provdes
    /// a way to have partially implemented EntityEvents, where only one or 
    /// more of the Interfaces are implemented.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class EntityEventWrapper<TEntity, TId> : IEntityEventAll<TEntity, TId>
        where TEntity : IId<TId>
    {
        #region Fields
        private readonly EntityEventContainer<TEntity, TId> _EntityEventContainer;
        private readonly TryLog _TryLog;
        #endregion

        #region Constructors
        public EntityEventWrapper(EntityEventContainer<TEntity, TId> entityEventContainer, 
                                  TryLog tryLog)
        {
            _EntityEventContainer = entityEventContainer 
                ?? throw new ArgumentNullException(nameof(entityEventContainer));
            _TryLog = tryLog ?? throw new ArgumentNullException(nameof(tryLog));
        }
        #endregion

        public void AfterDelete(TEntity entity, bool wasDeleted)
        {
            if (_EntityEventContainer.EntityEventAfterDelete != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterDelete.AfterDelete, entity, wasDeleted);
        }

        public void AfterDeleteMany(IEnumerable<TEntity> entities, Dictionary<TId, bool> wasDeleted)
        {
            if (_EntityEventContainer.EntityEventAfterPatch != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterDeleteMany.AfterDeleteMany, entities, wasDeleted);
        }

        public void AfterPatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison)
        {
            if (_EntityEventContainer.EntityEventAfterPatch != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterPatch.AfterPatch, patchedEntityComparison);
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntities)
        {
            if (_EntityEventContainer.EntityEventAfterPatch != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterPatchMany.AfterPatchMany, patchedEntities);
        }

        public void AfterPost(IEnumerable<TEntity> postedItems)
        {
            if (_EntityEventContainer.EntityEventAfterPost != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterPost.AfterPost, postedItems);
        }

        public void AfterPut(TEntity newEntity, TEntity priorEntity)
        {
            if (_EntityEventContainer.EntityEventAfterPut != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterPut.AfterPut, newEntity, priorEntity);
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
            if (_EntityEventContainer.EntityEventAfterUpdateProperty != null)
                _TryLog.Try(_EntityEventContainer.EntityEventAfterUpdateProperty.AfterUpdateProperty, property, newValue, existingValue);
        }

        public void BeforeDelete(TEntity entity)
        {
            if (_EntityEventContainer.EntityEventBeforeDelete != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforeDelete.BeforeDelete, entity);
        }

        public void BeforeDeleteMany(IEnumerable<TEntity> entities)
        {
            if (_EntityEventContainer.EntityEventBeforeDelete != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforeDeleteMany.BeforeDeleteMany, entities);
        }

        public void BeforePatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison)
        {
            if (_EntityEventContainer.EntityEventBeforePatch != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforePatch.BeforePatch, patchedEntityComparison);
        }

        public void BeforePatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> PatchedEntityComparisons)
        {
            if (_EntityEventContainer.EntityEventBeforePatch != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforePatchMany.BeforePatchMany, PatchedEntityComparisons);
        }

        public void BeforePost(IEnumerable<TEntity> postedItems)
        {
            if (_EntityEventContainer.EntityEventBeforePost != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforePost.BeforePost, postedItems);
        }

        public void BeforePut(TEntity newEntity, TEntity existingEntity)
        {
            if (_EntityEventContainer.EntityEventBeforePut != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforePut.BeforePut, newEntity, existingEntity);
        }

        public void BeforeUpdateProperty(string property, object newValue, object existingValue)
        {
            if (_EntityEventContainer.EntityEventBeforeUpdateProperty != null)
                _TryLog.Try(_EntityEventContainer.EntityEventBeforeUpdateProperty.BeforeUpdateProperty, property, newValue, existingValue);
        }
    }
}