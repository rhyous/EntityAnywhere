using LinqKit;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.WebServices
{
    public static class PropertyValueExtensions
    {
        public static Expression<Func<TEntity, bool>> ToExpression<TEntity, TInterface, TId>(this IEnumerable<PropertyValue> propertyValuePairs)
            where TEntity : class, TInterface, new()
            where TInterface : IExtensionEntity, IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
        {
            var expression = PredicateBuilder.New<TEntity>();
            var cache = new Dictionary<string, List<string>>();
            foreach (var propertyValue in propertyValuePairs)
            {
                if (cache.TryGetValue(propertyValue.Property, out List<string> values))
                {
                    values.Add(propertyValue.Value);
                    continue;
                }
                cache[propertyValue.Property] = new List<string> { propertyValue.Value };
            }
            foreach (var kvp in cache)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                expression.Or(e => e.Property == key && value.Contains(e.Value));
            }
            return expression;
        }
    }
}