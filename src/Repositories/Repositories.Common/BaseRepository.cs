using LinqKit;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// This is a common repository for all entities that will go into an Microsoft SQL Database using Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class BaseRepository<TEntity, TInterface, TId> : IRepository<TEntity, TInterface, TId>
        where TInterface : IEntity<TId>
        where TEntity : class, TInterface, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected virtual BaseDbContext<TEntity> DbContext
        {
            get { return _DbContext ?? (DbContext = new BaseDbContext<TEntity>()); }
            set { _DbContext = value; }
        } private BaseDbContext<TEntity> _DbContext;

        /// <inheritdoc />
        public virtual List<TInterface> Create(IEnumerable<TInterface> items)
        {
            List<TInterface> result = new List<TInterface>();
            if (items != null && items.Any())
            {
                var concrete = ConcreteConverter.ToConcrete<TEntity, TInterface>(items);
                result.AddRange(DbContext.Entities.AddRange(concrete));
                DbContext.SaveChanges();
            }
            return result;
        }

        /// <inheritdoc />
        public virtual bool Delete(TId id)
        {
            var item = DbContext.Entities.FirstOrDefault(o => o.Id.Equals(id));
            if (item == null)
                return true;
            DbContext.Entities.Remove(item);
            DbContext.SaveChanges();
            return true;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(bool order = false, string orderBy = "Id")
        {
            var query = DbContext.Entities.AsQueryable();
            if (order && !string.IsNullOrWhiteSpace(orderBy))
                query = query.OrderBy(orderBy.ToLambda<TEntity,TId>());
            return query;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(Expression<Func<TEntity, TId>> orderExpression)
        {
            var query = DbContext.Entities.AsQueryable();
            if (orderExpression != null)
                query = query.OrderBy(orderExpression);
            return query;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(IEnumerable<TId> ids)
        {
            return DbContext.Entities.Where(e => ids.Contains(e.Id));
        }

        /// <inheritdoc />
        public virtual TInterface Get(TId id)
        {
            return DbContext.Entities.FirstOrDefault(e => e.Id.Equals(id));
        }

        /// <inheritdoc />
        public virtual TInterface Get(string name, Expression<Func<TEntity, string>> propertyExpression)
        {
            return DbContext.Entities.AsExpandable().FirstOrDefault(e => propertyExpression.Invoke(e) == name);
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, string orderBy = "Id")
        {
            var query = DbContext.Entities.AsExpandable().Where(expression);
            if (!string.IsNullOrWhiteSpace(orderBy))
                query = query.OrderBy(orderBy.ToLambda<TEntity, TId>());
            return query;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TId>> orderExpression)
        {
            var query = DbContext.Entities.AsExpandable().Where(expression);
            if (orderExpression != null)
                query = query.OrderBy(orderExpression);
            return query;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Search(string searchString, params Expression<Func<TEntity, string>>[] propertyExpressions)
        {
            var predicate = PredicateBuilder.New<TEntity>();
            foreach (var expression in propertyExpressions)
            {
                predicate.Or(e => expression.Invoke(e).Contains(searchString));
            }
            return GetByExpression(predicate);
        }

        /// <inheritdoc />
        public virtual TInterface Update(TInterface item, IEnumerable<string> changedProperties, bool stage = false)
        {
            var existingItem = DbContext.Entities.FirstOrDefault(o => o.Id.Equals(item.Id));
            foreach (var prop in changedProperties)
            {
                var value = item.GetPropertyValue(prop);
                existingItem.GetPropertyInfo(prop).SetValue(existingItem, value);
            }
            if (!stage)
                DbContext.SaveChanges();
            return existingItem;
        }
    }
}
