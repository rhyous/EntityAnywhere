using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        protected readonly IServiceHandlerProvider<TEntity, TInterface, TId> _ServiceHandlerProvider;

        public ServiceCommon(IServiceHandlerProvider<TEntity, TInterface, TId> serviceHandlerProvider)
        {
            _ServiceHandlerProvider = serviceHandlerProvider;
        }

        /// <inheritdoc />
        public virtual async Task<int> GetCountAsync(NameValueCollection parameters = null) 
            => (await _ServiceHandlerProvider.QueryableHandler.GetQueryableAsync(parameters)).Count();

        /// <inheritdoc />
        public virtual async Task<IQueryable<TInterface>> GetAsync(IEnumerable<TId> ids, NameValueCollection parameters) 
            => await _ServiceHandlerProvider.GetByIdsHandler.GetAsync(ids, parameters);

        /// <inheritdoc />
        public virtual async Task<IQueryable<TInterface>> GetAsync(string property, IEnumerable<string> values, NameValueCollection parameters) 
            => await _ServiceHandlerProvider.GetByPropertyValuesHandler.GetAsync(property, values, parameters);

        /// <inheritdoc />
        public virtual List<TInterface> Get() => _ServiceHandlerProvider.QueryableHandler.GetQueryable().ToList();

        /// <inheritdoc />
        public virtual TInterface Get(TId id) => _ServiceHandlerProvider.GetByIdHandler.Get(id);

        /// <inheritdoc />
        public virtual async Task<IQueryable<TInterface>> GetAsync(NameValueCollection parameters) 
            => await _ServiceHandlerProvider.QueryableHandler.GetQueryableAsync(parameters);

        /// <inheritdoc />
        public virtual IQueryable<TInterface> Get(Expression<Func<TEntity, bool>> expression, int take = -1, int skip = -1) 
            => _ServiceHandlerProvider.QueryableHandler.GetQueryable(expression, take, skip);

        /// <inheritdoc />
        public virtual List<TInterface> Get(Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression) 
            => _ServiceHandlerProvider.QueryableHandler.GetQueryableWithModifier(queryableModifier, expression);

        /// <inheritdoc />
        public virtual string GetProperty(TId Id, string property) => _ServiceHandlerProvider.GetPropertyValueHandler.GetPropertyValue(property)?.ToString();

        /// <inheritdoc />
        public virtual string UpdateProperty(TId id, string property, string value) => _ServiceHandlerProvider.UpdateHandler.UpdateProperty(id, property, value);

        /// <inheritdoc />
        public virtual async Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities) => await _ServiceHandlerProvider.AddHandler.AddAsync(entities);

        /// <inheritdoc />
        /// <remarks>This must redirect to the  Add(IEnumerable<TInterface> entities) overload.</remarks>
        public virtual async Task<TInterface> AddAsync(TInterface entity) => (await AddAsync(new[] { entity })).FirstOrDefault();

        /// <inheritdoc />
        public virtual TInterface Update(TId id, PatchedEntity<TInterface, TId> patchedEntity) => _ServiceHandlerProvider.UpdateHandler.Update(id, patchedEntity);

        /// <inheritdoc />
        public virtual List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection) => _ServiceHandlerProvider.UpdateHandler.Update(patchedEntityCollection);

        /// <inheritdoc />
        public virtual TInterface Replace(TId id, TInterface entity) => _ServiceHandlerProvider.UpdateHandler.Replace(id, entity);

        /// <inheritdoc />
        public virtual bool Delete(TId id) => _ServiceHandlerProvider.DeleteHandler.Delete(id);

        /// <inheritdoc />
        public virtual Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids) => _ServiceHandlerProvider.DeleteHandler.DeleteMany(ids);

        /// <inheritdoc />
        public virtual RepositoryGenerationResult GenerateRepository() => _ServiceHandlerProvider.GenerateRepositoryHandler.GenerateRepository();

        /// <inheritdoc />
        public virtual RepositorySeedResult InsertSeedData() => _ServiceHandlerProvider.InsertSeedDataHandler.InsertSeedData();

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