using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    class UpdateHandler<TEntity, TInterface, TId> : IUpdateHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;
        private readonly IEntityInfo<TEntity> _EntityInfo;

        public UpdateHandler(IRepository<TEntity, TInterface, TId> repository,
                            IEntityInfo<TEntity> entityInfo)
        {
            _Repository = repository;
            _EntityInfo = entityInfo;
        }

        public TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity)
        {
            patchedEntity.Entity.Id = id; // Allows for copying entities from one to another
            return _Repository.Update(patchedEntity);
        }

        public List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection)
        {
            return _Repository.BulkUpdate(patchedEntityCollection);
        }

        public List<TInterface> Update(IEnumerable<TInterface> entities, params string[] changedProperties)
        {
            if (entities == null || !entities.Any() || changedProperties == null || !changedProperties.Any())
                return null;
            var patchedEntityCollection = new PatchedEntityCollection<TInterface, TId>();
            patchedEntityCollection.ChangedProperties.AddRange(changedProperties);
            patchedEntityCollection.PatchedEntities.AddRange(entities.Select(u => new PatchedEntity<TInterface, TId> { Entity = u }));
            return Update(patchedEntityCollection);
        }

        public string UpdateProperty(TId id, string property, string value)
        {
            var entity = new TEntity() { Id = id };
            var propertyType = entity.GetPropertyInfo(property).PropertyType;
            var typedValue = value.ToType(propertyType);
            entity.GetPropertyInfo(property).SetValue(entity, typedValue);
            var patchedEntity = new PatchedEntity<TInterface, TId>
            {
                Entity = entity,
                ChangedProperties = new HashSet<string> { property }
            };
            return Update(id, patchedEntity).GetPropertyValue(property).ToString();
        }

        public async Task<bool> UpdateStreamPropertyAsync(TId id, string property, Stream value)
        {
            var entity = new TEntity() { Id = id };
            var typedValue = value;
            return await _Repository.UpdateStreamPropertyAsync(id, property, value);
        }

        public TInterface Replace(TId id, TInterface entity)
        {
            entity.Id = id; // Allows for copying entities from one to another
            var allProperties = from prop in _EntityInfo.Properties.Values
                                where prop.CanRead && prop.CanWrite && prop.Name != "Id"
                                select prop.Name;
            var patchedEntity = new PatchedEntity<TInterface, TId> { Entity = entity, ChangedProperties = new HashSet<string>(allProperties) };
            return Update(id, patchedEntity);
        }
    }
}