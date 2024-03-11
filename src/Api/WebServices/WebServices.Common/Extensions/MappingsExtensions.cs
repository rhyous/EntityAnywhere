using LinqKit;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.StringLibrary;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.WebServices
{
    public static class MappingsExtensions
    {
        public static Expression<Func<TEntity, bool>> ToExpression<TEntity, TInterface, TId, TE1Id, TE2Id>(this IEnumerable<TEntity> mappings, IEntityInfoMapping<TEntity, TE1Id, TE2Id> entityInfoMapping)
            where TEntity : class, TInterface, new()
            where TInterface : IMappingEntity<TE1Id, TE2Id>, IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
            where TE1Id : IComparable, IComparable<TE1Id>, IEquatable<TE1Id>
            where TE2Id : IComparable, IComparable<TE2Id>, IEquatable<TE2Id>
        {
            var expression = PredicateBuilder.New<TEntity>();
            var cache = new Dictionary<TE1Id, List<TE2Id>>();
            foreach (var mapping in mappings)
            {
                var e1PropertyValue = (TE1Id)entityInfoMapping.E1PropertyInfo.GetValue(mapping, null);
                var e2PropertyValue = (TE2Id)entityInfoMapping.E2PropertyInfo.GetValue(mapping, null);
                if (cache.TryGetValue(e1PropertyValue, out List<TE2Id> idList))
                {
                    idList.Add(e2PropertyValue);
                    continue;
                }
                cache[e1PropertyValue] = new List<TE2Id> { e2PropertyValue };
            }
            foreach (var kvp in cache)
            {
                var exp1 = entityInfoMapping.E1PropertyInfo.Name.ToLambda<TEntity, TE1Id>(kvp.Key);
                var exp2 = entityInfoMapping.E2PropertyInfo.Name.ToLambda<TEntity, TE2Id>(kvp.Value);
                var subExpression = PredicateBuilder.New<TEntity>(exp1);
                subExpression.And(exp2);
                expression.Or(subExpression);
            }
            return expression;
        }
    }
}
