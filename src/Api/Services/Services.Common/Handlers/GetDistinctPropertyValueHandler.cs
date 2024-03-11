using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Services
{
    class GetDistinctPropertyValueHandler<TEntity, TInterface, TId> : IGetDistinctPropertyValuesHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;

        public GetDistinctPropertyValueHandler(IRepository<TEntity, TInterface, TId> repository)
        {
            _Repository = repository;
        }
        public List<object> Get(string propertyName, Expression<Func<TEntity, bool>> preExpression = null)
        {
            return _Repository.GetDistinctPropertyValues(propertyName, preExpression);
        }
    }
}