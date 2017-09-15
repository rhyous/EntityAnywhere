using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A common service layer for all entities that have a string property as a second key, other than the Id property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class ServiceCommonAlternateKey<TEntity, TInterface, TId> : ServiceCommon<TEntity, TInterface, TId>, IServiceCommonAlternateKey<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public ServiceCommonAlternateKey()
        {
            PropertyExpression = typeof(TEntity).GetAlternateKeyProperty().ToLambda<TEntity, string>();
        }
        
        public ServiceCommonAlternateKey(Expression<Func<TEntity, string>> propertyExpression)
        {
            PropertyExpression = propertyExpression;
        }

        /// <inheritdoc />
        public virtual Expression<Func<TEntity, string>> PropertyExpression { get; set; }

        /// <inheritdoc />
        public TInterface Get(string stringProperty)
        {
            return Repo.Get(stringProperty, PropertyExpression);
        }

        /// <inheritdoc />
        public List<TInterface> Search(string stringProperty)
        {
            return Repo.Search(stringProperty, PropertyExpression).ToList();
        }

        /// <inheritdoc />
        public override List<TInterface> Add(IList<TInterface> entities)
        {
            var duplicates = new List<string>();
            foreach (var entity in entities)
            {
                var method = PropertyExpression.Compile();
                var text = method(entity as TEntity);
                if (Get(text) != null)
                    duplicates.Add(text);
            }
            if (duplicates.Count > 0)
                throw new Exception($"Duplicate {typeof(TEntity).Name}(s) detected: {(string.Join(", ", duplicates))}");
            return base.Add(entities);
        }
    }
}