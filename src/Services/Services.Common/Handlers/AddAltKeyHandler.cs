using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    class AddAltKeyHandler<TEntity, TInterface, TId, TAltKey> : IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
           where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IEntityInfoAltKey<TEntity, TAltKey> _EntityInfoAltKey;
        private readonly IGetByPropertyValuesHandler<TEntity, TInterface, TId> _GetByPropertValuesHandler;
        private readonly IAddHandler<TEntity, TInterface, TId> _AddHandler;

        public AddAltKeyHandler(IEntityInfoAltKey<TEntity, TAltKey> entityInfoAltKey,
                                IGetByPropertyValuesHandler<TEntity, TInterface, TId> getByPropertValuesHandler,
                                IAddHandler<TEntity, TInterface, TId> addHandler)
        {
            _EntityInfoAltKey = entityInfoAltKey;
            _GetByPropertValuesHandler = getByPropertValuesHandler;
            _AddHandler = addHandler;
        }

        public async Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities)
        {            
            var method = _EntityInfoAltKey.PropertyExpressionMethod;
            var altKeys = entities.Select(e => method(e as TEntity)).ToList();
            var duplicates = (await _GetByPropertValuesHandler.GetAsync(_EntityInfoAltKey.AlternateKeyProperty, altKeys))?.ToList();
            if (duplicates != null && duplicates.Any())
                throw new DuplicateKeyException(_EntityInfoAltKey.AlternateKeyProperty, $"Duplicate {typeof(TEntity).Name}(s) detected: {(string.Join(", ", duplicates))}");
            return await _AddHandler.AddAsync(entities);
        }
    }
}