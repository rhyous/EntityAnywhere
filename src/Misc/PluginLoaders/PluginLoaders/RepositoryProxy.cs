using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.PluginLoaders
{
    public class RepositoryProxy<TEntity, TInterface, TId> : IRepository<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepositoryLoader<IRepository<TEntity, TInterface, TId>, TEntity, TInterface, TId> _RepositoryLoader;

        public RepositoryProxy(IRepositoryLoader<IRepository<TEntity, TInterface, TId>, TEntity, TInterface, TId> repositoryLoader)
        {
            _RepositoryLoader = repositoryLoader;
        }

        public IRepository<TEntity, TInterface, TId> Repo
        {
            get { return _Repo ?? (_Repo = _RepositoryLoader.LoadPlugin()); }
            set { _Repo = value; }
        } private IRepository<TEntity, TInterface, TId> _Repo;

        public IQueryable<TInterface> Get(string orderBy = "Id", SortOrder sortOrder = SortOrder.Ascending) 
            => Repo.Get(orderBy, sortOrder);

        public IQueryable<TInterface> Get<TProperty>(string orderBy = "Id", SortOrder sortOrder = SortOrder.Ascending)
            => Repo.Get<TProperty>(orderBy, sortOrder);

        public IQueryable<TInterface> Get(Expression<Func<TEntity, TId>> orderExpression)
            => Repo.Get(orderExpression);

        public IQueryable<TInterface> Get<TProperty>(Expression<Func<TEntity, TProperty>> orderExpression)
            => Repo.Get<TProperty>(orderExpression);

        public IQueryable<TInterface> Get(IEnumerable<TId> ids) => Repo.Get(ids);

        public TInterface Get(TId id) => Repo.Get(id);

        public TInterface Get<TResult>(TResult propertyValue, Expression<Func<TEntity, TResult>> propertyExpression)
            where TResult : IComparable, IComparable<TResult>, IEquatable<TResult> 
            => Repo.Get(propertyValue, propertyExpression);

        public IQueryable<TInterface> GetByExpression(Expression<Func<TEntity, bool>> expression, string orderBy = "Id", SortOrder sortOrder = SortOrder.Ascending) 
            => Repo.GetByExpression(expression, orderBy, sortOrder);

        public IQueryable<TInterface> GetByExpression<TProperty>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TProperty>> orderExpression, SortOrder sortOrder = SortOrder.Ascending)
            => Repo.GetByExpression(expression, orderExpression, sortOrder);

        public IQueryable<TInterface> Search<TResult>(TResult searchValue, params Expression<Func<TEntity, TResult>>[] propertyExpressions)
            => Repo.Search(searchValue, propertyExpressions);

        public List<TInterface> Create(IEnumerable<TInterface> entities) => Repo.Create(entities);

        public TInterface Update(PatchedEntity<TInterface, TId> patchedEntity, bool stage = false)
            => Repo.Update(patchedEntity, stage);

        public List<TInterface> BulkUpdate(PatchedEntityCollection<TInterface, TId> patchedEntityCollection, bool stage = false)
            => Repo.BulkUpdate(patchedEntityCollection, stage);

        public bool Delete(TId id) => Repo.Delete(id);

        public Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids) => Repo.DeleteMany(ids);

        public RepositoryGenerationResult GenerateRepository() => Repo.GenerateRepository();

        public void Dispose() => _Repo?.Dispose(); // Notice it uses backing field with null check, not property.
    }
}