using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A common service layer for all entities that have a string property as a second key, other than the Id property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class ServiceCommonAlternateKey<T, Tinterface, Tid> : ServiceCommon<T, Tinterface, Tid>, IServiceCommonAlternateKey<T, Tinterface, Tid>
        where T : class, Tinterface, new()
        where Tinterface : IEntity<Tid>
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        public ServiceCommonAlternateKey()
        {
            PropertyExpression = typeof(T).GetAlternateKeyProperty().ToLambda<Tinterface, string>();
        }
        
        public ServiceCommonAlternateKey(Expression<Func<Tinterface, string>> propertyExpression)
        {
            PropertyExpression = propertyExpression;
        }

        /// <inheritdoc />
        public virtual Expression<Func<Tinterface, string>> PropertyExpression { get; set; }

        /// <inheritdoc />
        public Tinterface Get(string stringProperty)
        {
            return Repo.Get(stringProperty, PropertyExpression);
        }

        /// <inheritdoc />
        public List<Tinterface> Search(string stringProperty)
        {
            return Repo.Search(stringProperty, PropertyExpression);
        }

        /// <inheritdoc />
        public override List<Tinterface> Add(IList<Tinterface> entities)
        {
            var duplicates = new List<string>();
            foreach (var entity in entities)
            {
                var method = PropertyExpression.Compile();
                var text = method(entity as T);
                if (Get(text) != null)
                    duplicates.Add(text);
            }
            if (duplicates.Count > 0)
                throw new Exception($"Duplicate {typeof(T).Name}(s) detected: {(string.Join(", ", duplicates))}");
            return base.Add(entities);
        }
    }
}