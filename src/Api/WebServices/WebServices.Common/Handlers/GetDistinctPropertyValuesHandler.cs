using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetDistinctPropertyValuesHandler<TEntity, TInterface, TId> : IGetDistinctPropertyValuesHandler<TEntity, TInterface, TId>
            where TEntity : class, TInterface, new()
            where TInterface : IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;

        public GetDistinctPropertyValuesHandler(IServiceCommon<TEntity, TInterface, TId> service)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public List<object> Handle(string propertyName, Expression<Func<TEntity, bool>> preExpression = null)
        {
            return _Service.GetDistinctPropertyValues(propertyName, preExpression);    
        }
    }
}
