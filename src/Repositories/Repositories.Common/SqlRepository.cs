using LinqKit;
using Rhyous.SimplePluginLoader;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// This is a common repository for all entities that will go into an Microsoft SQL Database using Entity Framework.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class SqlRepository<TEntity, TInterface, TId> : IRepository<TEntity, TInterface, TId>
        where TInterface : IBaseEntity<TId>
        where TEntity : class, TInterface, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly IObjectCreator<IBaseDbContext<TEntity>> _DbContextCreator;
        internal readonly List<IBaseDbContext<TEntity>> _UndisposedContexts = new List<IBaseDbContext<TEntity>>();
        const string IdProperty = "Id";

        public SqlRepository(IObjectCreator<IBaseDbContext<TEntity>> dbContextCreator)
        {
            _DbContextCreator = dbContextCreator;
        }

        /// <inheritdoc />
        public virtual List<TInterface> Create(IEnumerable<TInterface> items)
        {
            if (items == null || !items.Any())
                return null;

            List<TInterface> result = new List<TInterface>();
            using (var dbContext = CreateDbContext())
            {
                var concrete = ConcreteConverter.ToConcrete<TEntity, TInterface>(items);
                result.AddRange(dbContext.Entities.AddRange(concrete));
                dbContext.SaveChanges();
            }
            return result;
        }

        /// <inheritdoc />
        public virtual bool Delete(TId id)
        {
            using (var dbContext = CreateDbContext())
            {
                var item = dbContext.Entities.FirstOrDefault(o => o.Id.Equals(id));
                if (item == null)
                    return true;
                dbContext.Entities.Remove(item);
                dbContext.SaveChanges();
                return true;
            }
        }

        public virtual Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids)
        {
            if (ids is null || !ids.Any()) { throw new ArgumentNullException(nameof(ids)); }

            var distinctIds = ids.Distinct().ToList();
            var dictionary = new Dictionary<TId, bool>(distinctIds.Count);
            foreach (var id in distinctIds)
                dictionary.Add(id, true);
            using (var dbContext = CreateDbContext())
            {
                var entities = dbContext.Entities.Where(o => ids.Contains(o.Id));
                if (entities == null || !entities.Any())
                    return dictionary;
                dbContext.Entities.RemoveRange(entities);
                dbContext.SaveChanges();
                var notDeletedEntities = dbContext.Entities.Where(o => ids.Contains(o.Id));

                foreach (var notDeleted in notDeletedEntities)
                    dictionary[notDeleted.Id] = false;
                return dictionary;
            }
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(string orderBy = IdProperty, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = IdProperty;

            // If we are using the Id field, we know the type without reflection
            if (orderBy == IdProperty)
                return Get<TId>(orderBy, sortOrder);

            var propInfo = typeof(TEntity).GetProperty(orderBy);
            if (propInfo == null) { throw new ArgumentException($"The parameter must be a property of {typeof(TEntity).Name}", nameof(orderBy)); }

            // If we are using a property that returns a string, call it directly, to avoid reflection
            if (propInfo.PropertyType == typeof(string))
                return Get<string>(orderBy, sortOrder);
                        
            // Build the forward-to-method dynamically using reflection
            // This code allows for any parameters of any type. But it uses reflection, which isn't the fastest.
            var getMethodInfo = GetType().GetMethods()
                                         .FirstOrDefault(m => m.Name == nameof(Get)
                                                           && m.GetGenericArguments().Length == 1
                                                           && m.GetGenericArguments().First().Name == "TProperty"
                                                           && m.GetParameters().Length == 2);
            var getMethod = getMethodInfo.MakeGenericMethod(propInfo.PropertyType);
            try { return getMethod.Invoke(this, new object[] { orderBy, sortOrder }) as IQueryable<TInterface>; }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<TargetInvocationException, IQueryable<TInterface>>(e); }
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get<TProperty>(string orderBy, SortOrder sortOrder = SortOrder.Ascending)
        {
            var dbContext = CreateDbContext(); // No using as we may want to further change the queryable first
            _UndisposedContexts.Add(dbContext);
            var query = dbContext.Entities.AsQueryable();
            if (string.IsNullOrWhiteSpace(orderBy)) // Even with the default parameter, this is needed for empty or whitespace.
                orderBy = IdProperty;
            return sortOrder == SortOrder.Ascending
                 ? query.OrderBy(orderBy.ToLambda<TEntity, TProperty>())
                 : query.OrderByDescending(orderBy.ToLambda<TEntity, TProperty>());
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(Expression<Func<TEntity, TId>> orderExpression)
            => Get<TId>(orderExpression);

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get<TProperty>(Expression<Func<TEntity, TProperty>> orderExpression)
        {
            var dbContext = CreateDbContext();
            _UndisposedContexts.Add(dbContext);
            var query = dbContext.Entities.AsQueryable();
            if (orderExpression != null)
                query = query.OrderBy(orderExpression);
            return query;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(IEnumerable<TId> ids)
        {
            if (ids == null || !ids.Any()) { throw new ArgumentException(nameof(ids)); }
            var dbContext = CreateDbContext();
            _UndisposedContexts.Add(dbContext);
            return dbContext.Entities.Where(e => ids.Contains(e.Id));
        }

        /// <inheritdoc />
        public virtual TInterface Get(TId id)
        {
            using (var dbContext = CreateDbContext())
            {
                return dbContext.Entities.FirstOrDefault(e => e.Id.Equals(id));
            }
        }

        /// <inheritdoc />
        public virtual TInterface Get<TResult>(TResult propertyValue, Expression<Func<TEntity, TResult>> propertyExpression)
            where TResult : IComparable, IComparable<TResult>, IEquatable<TResult>
        {
            if (propertyExpression is null) { throw new ArgumentNullException(nameof(propertyExpression)); }
            using (var dbContext = CreateDbContext())
            {
                try { return dbContext.Entities.AsExpandable().FirstOrDefault(e => propertyExpression.Invoke(e).Equals(propertyValue)); }
                catch (Exception) { throw; }
            }
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, string orderBy = IdProperty, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (expression == null) { throw new ArgumentNullException(nameof(expression)); }
            if (string.IsNullOrWhiteSpace(orderBy)) { orderBy = IdProperty; } // Even with the default parameter, this is needed for empty or whitespace.
            if (orderBy == IdProperty)
                return GetByExpression<TId>(expression, orderBy.ToLambda<TEntity, TId>(), sortOrder);
            var propInfo = typeof(TEntity).GetProperty(orderBy);
            if (propInfo == null) { throw new ArgumentException($"The parameter must be a property of {typeof(TEntity).Name}", nameof(orderBy)); }

            // If we are using a property that returns a string, call it directly, to avoid reflection
            if (propInfo.PropertyType == typeof(string))
                return GetByExpression<string>(expression, orderBy.ToLambda<TEntity, string>(), sortOrder);

            // Build the expression dynamically using reflection
            object orderByExpression = orderBy.GetOrderByExpression<TEntity>(propInfo);

            // Build the forward-to-method dynamically using reflection
            var getByExpressionMethodInfo = GetType().GetMethods()
                                            .FirstOrDefault(m => m.Name == nameof(GetByExpression)
                                                              && m.GetGenericArguments().Length == 1
                                                              && m.GetParameters().Length == 3);
            var getByExpression = getByExpressionMethodInfo.MakeGenericMethod(propInfo.PropertyType);

            try { return getByExpression.Invoke(this, new[] { expression, orderByExpression, sortOrder }) as IQueryable<TInterface>; }
            catch (Exception e) { return ExceptionUnwrapper.HandleException<TargetInvocationException, IQueryable<TInterface>>(e); }
        }

        /// <summary>
        /// This method allows you to use any property to order by. This only works if the TEntity is an actual Entity. In the case of an IAuditable it will not work
        /// </summary>
        /// <typeparam name="TProperty">The property type</typeparam>
        /// <param name="expression">The Where expression</param>
        /// <param name="orderExpression">The OrderBy expression</param>
        /// <returns><see cref="IQueryable{T}"/> of results</returns>
        public virtual IQueryable<TInterface> GetByExpression<TProperty>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TProperty>> orderExpression, SortOrder sortOrder)
        {
            if (expression is null) { throw new ArgumentNullException(nameof(expression)); }
            var dbContext = CreateDbContext();
            _UndisposedContexts.Add(dbContext);
            var query = dbContext.Entities.AsExpandable().Where(expression);
            if (orderExpression != null)
            {
                if (sortOrder == SortOrder.Ascending)
                    query = query.OrderBy(orderExpression);
                else
                    query = query.OrderByDescending(orderExpression);
            }
            return query;
        }

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Search<TResult>(TResult searchValue, params Expression<Func<TEntity, TResult>>[] propertyExpressions)
        {
            if (propertyExpressions is null) { throw new ArgumentNullException(nameof(propertyExpressions)); }

            var predicate = PredicateBuilder.New<TEntity>();
            foreach (var expression in propertyExpressions)
            {
                predicate.Or(e => expression.Invoke(e).ToString().Contains(searchValue.ToString()));
            }
            return GetByExpression(predicate);
        }

        /// <inheritdoc />
        public virtual TInterface Update(PatchedEntity<TInterface, TId> patchedEntity, bool stage = false)
        {
            if (patchedEntity is null) { throw new ArgumentNullException(nameof(patchedEntity)); }
            return BulkUpdate(new PatchedEntityCollection<TInterface, TId> { PatchedEntities = new List<PatchedEntity<TInterface, TId>> { patchedEntity } }, stage)
                          .FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual List<TInterface> BulkUpdate(PatchedEntityCollection<TInterface, TId> patchedEntityCollection, bool stage = false)
        {
            if (patchedEntityCollection == null || patchedEntityCollection.PatchedEntities == null)
                throw new ArgumentNullException(nameof(patchedEntityCollection));
            if (!patchedEntityCollection.PatchedEntities.Any())
                throw new ArgumentException($"The type {patchedEntityCollection.GetType()} must have a list of {nameof(PatchedEntityCollection<TInterface, TId>.PatchedEntities)}.", nameof(patchedEntityCollection));
            if (patchedEntityCollection.PatchedEntities.Any(pe => pe is null))
                throw new ArgumentNullException($"The parameter {patchedEntityCollection.GetType()} is not null, but one of the items in {nameof(patchedEntityCollection)}.{nameof(PatchedEntityCollection<TInterface, TId>.PatchedEntities)} is null.", nameof(patchedEntityCollection));
            var updatedEntities = new List<TInterface>();
            using (var dbContext = CreateDbContext(typeof(IUpdateDbContext<TEntity>)))
            {
                foreach (var patchedEntity in patchedEntityCollection.PatchedEntities)
                {
                    var entity = patchedEntity.Entity.ToConcrete<TEntity, TInterface>();
                    dbContext.Entities.Attach(entity);
                    if (patchedEntityCollection.ChangedProperties != null && patchedEntityCollection.ChangedProperties.Any())
                    {
                        patchedEntity.ChangedProperties = patchedEntity.ChangedProperties ?? new HashSet<string>();
                        patchedEntity.ChangedProperties.UnionWith(patchedEntityCollection.ChangedProperties);
                    }
                    foreach (var prop in patchedEntity.ChangedProperties)
                    {
                        try { dbContext.SetIsModified(entity, prop, true); }
                        catch (Exception e) { throw e; }
                    }
                    updatedEntities.Add(entity);
                }
                if (!stage)
                    dbContext.SaveChanges();
                return updatedEntities;
            }
        }

        public virtual RepositoryGenerationResult GenerateRepository()
        {
            using (var dbContext = CreateDbContext())
            {
                var result = new RepositoryGenerationResult { Name = typeof(TEntity).Name };
                try
                {
                    var count = dbContext.Entities.Count();
                    result.RepositoryReady = true;
                    return result;
                }
                catch (Exception e)
                {
                    result.RepositoryReady = false;
                    result.FailureReason = e.Message;
                    return result;
                }
            }
        }

        protected virtual IBaseDbContext<TEntity> CreateDbContext(Type customDbContextType = null)
        {
            return _DbContextCreator.Create(customDbContextType ?? typeof(IBaseDbContext<TEntity>));
        }

        #region IDisposable

        protected bool IsDisposed;

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                if (disposing)
                {
                    foreach (var undisposed in _UndisposedContexts)
                    {
                        undisposed.Dispose();
                    }
                }
            }
        }

        #endregion
    }
}