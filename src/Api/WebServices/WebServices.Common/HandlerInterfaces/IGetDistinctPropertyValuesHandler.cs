using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGetDistinctPropertyValuesHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        List<object> Handle(string propertyName, Expression<Func<TEntity, bool>> preExpression = null);
    }
}