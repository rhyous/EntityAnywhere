using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    /// <summary>
    /// This is a proxy for the DI container to register against IServiceCommon<TEntity, TInterface, TId>.
    /// This will allow the plugin loader
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    /// <typeparam name="TInterface">The entity interface.</typeparam>
    /// <typeparam name="TId">The type of the Id property.</typeparam>
    public class ServiceProxy<TEntity, TInterface, TId> : IServiceCommon<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly IEntityServiceLoader<IServiceCommon<TEntity, TInterface, TId>, TEntity, TInterface, TId> _ServiceLoader;

        public ServiceProxy(IEntityServiceLoader<IServiceCommon<TEntity, TInterface, TId>, TEntity, TInterface, TId> serviceLoader)
        {
            _ServiceLoader = serviceLoader ?? throw new ArgumentNullException(nameof(serviceLoader));
        }

        protected virtual IServiceCommon<TEntity, TInterface, TId> Service
        {
            get { return _Service ?? (_Service = _ServiceLoader.LoadPlugin()); }
            set { _Service = value; }
        } protected IServiceCommon<TEntity, TInterface, TId> _Service;

        public async Task<int> GetCountAsync(NameValueCollection parameters = null) => await Service.GetCountAsync(parameters);

        public List<TInterface> Get() => Service.Get();

        public async Task<IQueryable<TInterface>> GetAsync(NameValueCollection parameters) => await Service.GetAsync(parameters);

        public async Task<IQueryable<TInterface>> GetAsync(IEnumerable<TId> ids, NameValueCollection parameters) 
            => await Service.GetAsync(ids, parameters);

        public async Task<IQueryable<TInterface>> GetAsync(string property, IEnumerable<string> values, NameValueCollection parameters)
            => await Service.GetAsync(property, values, parameters);

        public TInterface Get(TId id) => Service.Get(id);

        public IQueryable<TInterface> Get(Expression<Func<TEntity, bool>> expression, int take = -1, int skip = -1) 
                                      => Service.Get(expression, take, skip);

        public List<TInterface> Get(Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression) 
                                => Service.Get(queryableModifier, expression);

        public virtual List<object> GetDistinctPropertyValues(string propertyName, Expression<Func<TEntity, bool>> preExpression = null)
            => Service.GetDistinctPropertyValues(propertyName, preExpression);

        public string GetPropertyValue(TId id, string property) => Service.GetPropertyValue(id, property);

        public string UpdateProperty(TId id, string property, string value) => Service.UpdateProperty(id, property, value);

        public async Task<bool> UpdateStreamPropertyAsync(TId id, string property, Stream value)
            => await Service.UpdateStreamPropertyAsync(id, property, value);

        public TInterface Update(TId id, PatchedEntity<TInterface, TId> pathcedEntity) => Service.Update(id, pathcedEntity);

        public List<TInterface> Update(PatchedEntityCollection<TInterface, TId> patchedEntityCollection) 
                                => Service.Update(patchedEntityCollection);

        public async Task<TInterface> AddAsync(TInterface entity) => await Service.AddAsync(entity);

        public async Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities) => await Service.AddAsync(entities);

        public TInterface Replace(TId id, TInterface entity) => Service.Replace(id, entity);

        public bool Delete(TId id) => Service.Delete(id);

        public Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids) => Service.DeleteMany(ids);

        public RepositoryGenerationResult GenerateRepository() => Service.GenerateRepository();

        public RepositorySeedResult InsertSeedData() => Service.InsertSeedData();

        public void Dispose() => _Service?.Dispose(); // Notice it uses backing field with null check, not property.
    }
}