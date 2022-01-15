using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    public class EntityEventProxy<TEntity, TId> : IEntityEventAll<TEntity, TId>
        where TEntity : class, IId<TId>
    {
        private readonly EntityEventLoader<TEntity, TId> _EntityEventLoader;

        public EntityEventProxy(EntityEventLoader<TEntity, TId> entityEventLoader)
        {
            _EntityEventLoader = entityEventLoader ?? throw new ArgumentNullException(nameof(entityEventLoader));
        }
        internal protected virtual IEntityEventAll<TEntity, TId> Event
        {
            get { return _Event ?? (_Event = _EntityEventLoader.LoadPlugins()); }
            set { _Event = value; }
        } private IEntityEventAll<TEntity, TId> _Event;

        public void BeforePatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison) 
                    => Event?.BeforePatch(patchedEntityComparison);
        public void AfterPatch(PatchedEntityComparison<TEntity, TId> PatchedEntityComparison)
                    => Event?.AfterPatch(PatchedEntityComparison);
        public void BeforePatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons)
                    => Event?.BeforePatchMany(patchedEntityComparisons);
        public void AfterPatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons)
                    => Event?.AfterPatchMany(patchedEntityComparisons);
        public void BeforePost(IEnumerable<TEntity> postedItems) => Event?.BeforePost(postedItems);

        public void AfterPost(IEnumerable<TEntity> postedItems) => Event?.AfterPost(postedItems);

        public void BeforePut(TEntity newEntity, TEntity existingEntity) => Event?.BeforePut(newEntity, existingEntity);

        public void AfterPut(TEntity newEntity, TEntity priorEntity) => Event?.AfterPut(newEntity, priorEntity);

        public void BeforeUpdateProperty(string property, object newValue, object existingValue) 
                    => Event?.BeforeUpdateProperty(property, newValue, existingValue);

        public void AfterUpdateProperty(string property, object newValue, object existingValue)
                    => Event?.AfterUpdateProperty(property, newValue, existingValue);

        public void BeforeDelete(TEntity entity) => Event?.BeforeDelete(entity);

        public void AfterDelete(TEntity entity, bool wasDeleted) => Event?.AfterDelete(entity, wasDeleted);

        public void BeforeDeleteMany(IEnumerable<TEntity> entities) => Event?.BeforeDeleteMany(entities);

        public void AfterDeleteMany(IEnumerable<TEntity> entities, Dictionary<TId, bool> wasDeleted) 
                    => Event?.AfterDeleteMany(entities, wasDeleted);
    }
}
