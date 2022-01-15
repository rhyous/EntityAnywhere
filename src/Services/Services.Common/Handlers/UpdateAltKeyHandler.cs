using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    class UpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey> : IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
           where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IEntityInfoAltKey<TEntity, TAltKey> _EntityInfoAltKey;
        private readonly IGetByPropertyValuesHandler<TEntity, TInterface, TId> _GetByPropertyValuesHandler;
        private readonly IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> _GetByAlternateKeyHandler;
        private readonly IUpdateHandler<TEntity, TInterface, TId> _UpdateHandler;

        public UpdateAltKeyHandler(IEntityInfoAltKey<TEntity, TAltKey> entityInfoAltKey,
                                   IGetByPropertyValuesHandler<TEntity, TInterface, TId> getByPropertyValuesHandler,
                                   IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> getByAlternateKeyHandler,
                                   IUpdateHandler<TEntity, TInterface, TId> UpdateHandler)
        {
            _EntityInfoAltKey = entityInfoAltKey;
            _GetByPropertyValuesHandler = getByPropertyValuesHandler;
            _GetByAlternateKeyHandler = getByAlternateKeyHandler;
            _UpdateHandler = UpdateHandler;
        }

        public TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity)

        {
            // If we are updating the AlternateId property, check for duplicates.
            if (patchedEntity.ChangedProperties.Contains(_EntityInfoAltKey.AlternateKeyProperty, StringComparer.OrdinalIgnoreCase))
            {
                var method = _EntityInfoAltKey.PropertyExpressionMethod;
                var altKey = method(patchedEntity.Entity as TEntity);
                var existingEntity = _GetByAlternateKeyHandler.Get(altKey);
                if (existingEntity != null && !existingEntity.Id.Equals(id))
                    throw new DuplicateKeyException(_EntityInfoAltKey.AlternateKeyProperty, $"Duplicate {typeof(TEntity).Name}(s) detected: {altKey}");
            }
            return _UpdateHandler.Update(id, patchedEntity);
        }

        public List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection)

        {
            // TODO: We should prevent duplicates but call the repo in a Big O(1) way.
            // TODO: We should be able to swap AltKey's in two entities. A == B and B == A.
            return _UpdateHandler.Update(patchedEntityCollection);
        }
    }
}