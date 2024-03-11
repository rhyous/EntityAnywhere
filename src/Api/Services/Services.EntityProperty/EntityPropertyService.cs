using Rhyous.Collections;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public class EntityPropertyService
        : ServiceCommon<EntityProperty, IEntityProperty, int>,
          IEntityPropertyService
    {
        private readonly IPropertyOrderSorter _PropertyOrderSorter;

        public EntityPropertyService(IServiceHandlerProvider serviceHandlerProvider,
                                     IPropertyOrderSorter propertyOrderSorter) 
            : base(serviceHandlerProvider)
        {
            _PropertyOrderSorter = propertyOrderSorter;
        }

        /// <summary>
        /// Deletes the EntityProperty. This override specifically reorders the EntityProperty.Order, so if you have 5 EntityProperties, 
        /// ordered 1, 2, 3, 4, 5 and you delelete 3, 4 and 5 are reorders to 3 and 4.
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns>True if deleted or not found, false if it can't be deleted.</returns>
        public override bool Delete(int id)
        {
            var existing = _ServiceHandlerProvider.Provide<IGetByIdHandler<EntityProperty, IEntityProperty, int>>().Get(id);
            if (existing == null)
                return true;
            var entityId = existing.EntityId;
            var result = _ServiceHandlerProvider.Provide<IDeleteHandler<EntityProperty, IEntityProperty, int>>().Delete(id);
            if (!result)
                return result;
            var remaining = _ServiceHandlerProvider.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>().GetQueryable(e => e.EntityId == entityId, -1, -1, nameof(EntityProperty.Order), SortOrder.Ascending).ToList();
            var updated = _PropertyOrderSorter.UpdateSortOrder(remaining, p => p.Name, PreferentialPropertyComparer.Instance);
            _ServiceHandlerProvider.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>().Update(updated, nameof(EntityProperty.Order));
            return result;
        }

        /// <summary>
        /// Adds EntityProperties.  This override specifically reorders the EntityProperty.Order, so if you have 3 orders, 
        /// ordered 1, 2, 3 and you add two new ones at orders 1 and 3, the added Order 1 becomes 1, the existing 1 becomes 2,
        /// the added Order 3 becomes 3, the existing 2 and 3 become 4 and 5. However, you can add EntityProperties from
        /// different entities at the same time and this handles that, doing the reording of each group.
        /// </summary>
        /// <param name="entityProperties">The EntityProperties to add.</param>
        /// <returns>The Added entity properties.</returns>
        public override async Task<List<IEntityProperty>> AddAsync(IEnumerable<IEntityProperty> entityProperties)
        {
            var parentEntityIds = entityProperties.Select(e => e.EntityId);
            var existingEntityProperties = _ServiceHandlerProvider.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>().GetQueryable(e => parentEntityIds.Contains(e.EntityId));
            var existingMap = _PropertyOrderSorter.Sort(existingEntityProperties, e => e.EntityId, e => e.Order, e => e.Name);
            var addMap = _PropertyOrderSorter.Sort(entityProperties, e => e.EntityId, e => e.Order, e => e.Name);
            _PropertyOrderSorter.Collate(existingMap, addMap, e => e.EntityId);
            var updated = new List<IEntityProperty>();
            foreach (var kvp in existingMap)
                updated.AddRange(_PropertyOrderSorter.UpdateSortOrder(kvp.Value, x => x.Name, PreferentialPropertyComparer.Instance));
            entityProperties = entityProperties.OrderBy(e => e.EntityId).ThenBy(e => e.Order);
            var result = await base.AddAsync(entityProperties);
            updated.RemoveAny(entityProperties);
            if (updated.Any())
                _ServiceHandlerProvider.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>().Update(updated, nameof(EntityProperty.Order));
            return result;
        }        

        public override IEntityProperty Update(int id, PatchedEntity<IEntityProperty, int> patchedEntity)
        {
            if (!patchedEntity.ChangedProperties.Contains("Order", StringComparer.OrdinalIgnoreCase))
            {
                return base.Update(id, patchedEntity);
            }
            var prior = _ServiceHandlerProvider.Provide<IGetByIdHandler<EntityProperty, IEntityProperty, int>>().Get(id);
            var existing = _ServiceHandlerProvider.Provide<IQueryableHandler<EntityProperty, IEntityProperty, int>>().GetQueryable(e => e.EntityId == prior.EntityId && e.Id != prior.Id, -1, -1, nameof(EntityProperty.Order))
                                                  .ToList();
            _PropertyOrderSorter.SafeInsert(existing, patchedEntity.Entity);
            var updated = _PropertyOrderSorter.UpdateSortOrder(existing, p => p.Name, PreferentialPropertyComparer.Instance).ToList();
            var result = base.Update(id, patchedEntity);
            updated.Remove(result);
            if (updated.Any())
                _ServiceHandlerProvider.Provide<IUpdateHandler<EntityProperty, IEntityProperty, int>>().Update(updated, nameof(EntityProperty.Order));
            return result;
        }
    }
}
