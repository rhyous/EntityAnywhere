using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    class GetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> : IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;
        private readonly IEntityInfoAltKey<TEntity, TAltKey> _EntityInfoAltKey;

        public GetByAlternateKeyHandler(IRepository<TEntity, TInterface, TId> repository,
                                        IEntityInfoAltKey<TEntity, TAltKey> entityInfoAltKey)
        {
            _Repository = repository;
            _EntityInfoAltKey = entityInfoAltKey;
        }


        public TInterface Get(TAltKey propertyValue)
        { 
            return _Repository.Get(propertyValue, _EntityInfoAltKey.PropertyExpression);
        }
    }
}