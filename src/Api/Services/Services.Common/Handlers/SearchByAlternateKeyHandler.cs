using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    class SearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> : ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;
        private readonly IEntityInfoAltKey<TEntity, TAltKey> _EntityInfoAltKey;

        public SearchByAlternateKeyHandler(IRepository<TEntity, TInterface, TId> repository,
                                           IEntityInfoAltKey<TEntity, TAltKey> entityInfoAltKey)
        {
            _Repository = repository;
            _EntityInfoAltKey = entityInfoAltKey;
        }

        public List<TInterface> Search(TAltKey propertyValue)
        { 
            return _Repository.Search(propertyValue, _EntityInfoAltKey.PropertyExpression).ToList();
        }
    }
}