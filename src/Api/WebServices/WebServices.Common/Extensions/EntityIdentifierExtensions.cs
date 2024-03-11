using LinqKit;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.WebServices
{
    public static class EntityIdentifierExtensions
    {
        public static Expression<Func<TEntity, bool>> ToExpression<TEntity, TInterface, TId>(this IEnumerable<EntityIdentifier> entityIdentifiers)
            where TEntity : class, TInterface, new()
            where TInterface : IExtensionEntity, IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
        {
            var expression = PredicateBuilder.New<TEntity>();
            var cache = new Dictionary<string, List<string>>();
            foreach (var identifier in entityIdentifiers)
            {
                if (cache.TryGetValue(identifier.Entity, out List<string> idList))
                {
                    idList.Add(identifier.EntityId);
                    continue;
                }
                cache[identifier.Entity] = new List<string> { identifier.EntityId };
            }
            foreach (var kvp in cache)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                expression.Or(e => e.Entity == key && value.Contains(e.EntityId));
            }
            return expression;
        }
    }
}
