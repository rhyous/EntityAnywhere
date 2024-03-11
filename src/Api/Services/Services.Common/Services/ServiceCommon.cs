using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// A common service layer for all Entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class ServiceCommon<TEntity, TInterface, TId>
           : IServiceCommon<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly IServiceHandlerProvider _ServiceHandlerProvider;

        public ServiceCommon(IServiceHandlerProvider serviceHandlerProvider)
        {
            _ServiceHandlerProvider = serviceHandlerProvider;
        }

        /// <inheritdoc />
        public virtual async Task<int> GetCountAsync(NameValueCollection parameters = null)
            => (await _ServiceHandlerProvider.Provide<IQueryableHandler<TEntity, TInterface, TId>>()
                                             .GetQueryableAsync(parameters)).Count();

        /// <inheritdoc />
        public virtual async Task<IQueryable<TInterface>> GetAsync(IEnumerable<TId> ids, NameValueCollection parameters)
            => await _ServiceHandlerProvider.Provide<IGetByIdsHandler<TEntity, TInterface, TId>>()
                                            .GetAsync(ids, parameters);

        /// <inheritdoc />
        public virtual async Task<IQueryable<TInterface>> GetAsync(string property, IEnumerable<string> values, NameValueCollection parameters)
            => await _ServiceHandlerProvider.Provide<IGetByPropertyValuesHandler<TEntity, TInterface, TId>>()
                                            .GetAsync(property, values, parameters);

        /// <inheritdoc />
        public virtual List<TInterface> Get()
            => _ServiceHandlerProvider.Provide<IQueryableHandler<TEntity, TInterface, TId>>()
                                      .GetQueryable().ToList();

        /// <inheritdoc />
        public virtual TInterface Get(TId id)
            => _ServiceHandlerProvider.Provide<IGetByIdHandler<TEntity, TInterface, TId>>()
                                      .Get(id);

        /// <inheritdoc />
        public virtual async Task<IQueryable<TInterface>> GetAsync(NameValueCollection parameters)
            => await _ServiceHandlerProvider.Provide<IQueryableHandler<TEntity, TInterface, TId>>()
                                            .GetQueryableAsync(parameters);

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(Expression<Func<TEntity, bool>> expression, int take = -1, int skip = -1)
            => _ServiceHandlerProvider.Provide<IQueryableHandler<TEntity, TInterface, TId>>().GetQueryable(expression, take, skip);

        /// <inheritdoc />
        public virtual List<TInterface> Get(Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression)
            => _ServiceHandlerProvider.Provide<IQueryableHandler<TEntity, TInterface, TId>>()
                                      .GetQueryableWithModifier(queryableModifier, expression);

        /// <inheritdoc />
        public virtual string GetPropertyValue(TId id, string property)
            => _ServiceHandlerProvider.Provide<IGetPropertyValueHandler<TEntity, TInterface, TId>>()
                                      .GetSelectProperty(id, property);

        /// <inheritdoc />
        public virtual List<object> GetDistinctPropertyValues(string propertyName, Expression<Func<TEntity, bool>> preExpression = null)
            => _ServiceHandlerProvider.Provide<IGetDistinctPropertyValuesHandler<TEntity, TInterface, TId>>()
                                      .Get(propertyName, preExpression);

        /// <inheritdoc />
        public virtual string UpdateProperty(TId id, string property, string value)
            => _ServiceHandlerProvider.Provide<IUpdateHandler<TEntity, TInterface, TId>>()
                                      .UpdateProperty(id, property, value);

        /// <inheritdoc />
        public virtual async Task<bool> UpdateStreamPropertyAsync(TId id, string property, Stream value)
            => await _ServiceHandlerProvider.Provide<IUpdateHandler<TEntity, TInterface, TId>>()
                                            .UpdateStreamPropertyAsync(id, property, value);

        /// <inheritdoc />
        public virtual async Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities)
            => await _ServiceHandlerProvider.Provide<IAddHandler<TEntity, TInterface, TId>>()
                                            .AddAsync(entities);

        /// <inheritdoc />
        /// <remarks>This must redirect to the  Add(IEnumerable<TInterface> entities) overload.</remarks>
        public virtual async Task<TInterface> AddAsync(TInterface entity)
            => (await AddAsync(new[] { entity })).FirstOrDefault();

        /// <inheritdoc />
        public virtual TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity)
            => _ServiceHandlerProvider.Provide<IUpdateHandler<TEntity, TInterface, TId>>()
                                      .Update(id, patchedEntity);

        /// <inheritdoc />
        public virtual List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection)
            => _ServiceHandlerProvider.Provide<IUpdateHandler<TEntity, TInterface, TId>>()
                                      .Update(patchedEntityCollection);

        /// <inheritdoc />
        public virtual TInterface Replace(TId id, TInterface entity)
            => _ServiceHandlerProvider.Provide<IUpdateHandler<TEntity, TInterface, TId>>()
                                     .Replace(id, entity);

        /// <inheritdoc />
        public virtual bool Delete(TId id)
            => _ServiceHandlerProvider.Provide<IDeleteHandler<TEntity, TInterface, TId>>()
                                      .Delete(id);

        /// <inheritdoc />
        public virtual Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids)
            => _ServiceHandlerProvider.Provide<IDeleteHandler<TEntity, TInterface, TId>>()
                                      .DeleteMany(ids);

        /// <inheritdoc />
        public virtual RepositoryGenerationResult GenerateRepository()
            => _ServiceHandlerProvider.Provide<IGenerateRepositoryHandler<TEntity, TInterface, TId>>()
                                      .GenerateRepository();

        /// <inheritdoc />
        public virtual RepositorySeedResult InsertSeedData()
            => _ServiceHandlerProvider.Provide<IInsertSeedDataHandler<TEntity, TInterface, TId>>()
                                      .InsertSeedData();

        #region IDisposable

        private bool _IsDisposed;

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                if (disposing)
                {
                }
                // Dispose unmanaged resources
            }
        }

        #endregion
    }
}