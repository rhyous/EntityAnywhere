using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Events
{
    /// <summary>
    /// Events for T.
    /// </summary>
    public class CommonEvent<TEntity, TId> : IEntityEventAll<TEntity, TId>
        where TEntity : IId<TId>
    {
        private readonly ILogger _Logger;

        public CommonEvent(ILogger logger)
        {
            _Logger = logger;
        }

        #region Before
        public void BeforeDelete(TEntity entity)
        {
        }

        public void BeforeDeleteMany(IEnumerable<TEntity> entities)
        {
        }

        public void BeforePatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison)
        {
        }

        public void BeforePatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons)
        {
        }

        public void BeforePost(IEnumerable<TEntity> postedItems)
        {
        }

        public void BeforePut(TEntity newEntity, TEntity existingEntity)
        {
        }

        public void BeforeUpdateProperty(string property, object newValue, object existingValue)
        {
        }
        #endregion

        #region After
        public void AfterDelete(TEntity T, bool wasDeleted)
        {
        }

        public void AfterDeleteMany(IEnumerable<TEntity> entities, Dictionary<TId, bool> wasDeleted)
        {

        }

        public void AfterPatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison)
        {
        }

        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons)
        {
        }

        public void AfterPost(IEnumerable<TEntity> postedItems)
        {
        }

        public void AfterPut(TEntity newEntity, TEntity priorEntity)
        {
        }

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
        {
        }
        #endregion
    }
}