using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class ServiceHandlerProvider<TEntity, TInterface, TId> : IServiceHandlerProvider<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly ILifetimeScope _Container;

        public ServiceHandlerProvider(ILifetimeScope container)
        {
            _Container = container;
        }

        public IAddHandler<TEntity, TInterface, TId> AddHandler => _Container.Resolve<IAddHandler<TEntity, TInterface, TId>>();
        public IDeleteHandler<TEntity, TInterface, TId> DeleteHandler => _Container.Resolve<IDeleteHandler<TEntity, TInterface, TId>>();
        public IGenerateRepositoryHandler<TEntity, TInterface, TId> GenerateRepositoryHandler => _Container.Resolve<IGenerateRepositoryHandler<TEntity, TInterface, TId>>();
        public IInsertSeedDataHandler<TEntity, TInterface, TId> InsertSeedDataHandler => _Container.Resolve<IInsertSeedDataHandler<TEntity, TInterface, TId>>();
        public IQueryableHandler<TEntity, TInterface, TId> QueryableHandler => _Container.Resolve<IQueryableHandler<TEntity, TInterface, TId>>();
        public IGetByIdHandler<TEntity, TInterface, TId> GetByIdHandler => _Container.Resolve<IGetByIdHandler<TEntity, TInterface, TId>>();
        public IGetByIdsHandler<TEntity, TInterface, TId> GetByIdsHandler => _Container.Resolve<IGetByIdsHandler<TEntity, TInterface, TId>>();
        public IGetPropertyValueHandler<TEntity, TInterface, TId> GetPropertyValueHandler => _Container.Resolve<IGetPropertyValueHandler<TEntity, TInterface, TId>>();
        public IGetByPropertyValuesHandler<TEntity, TInterface, TId> GetByPropertyValuesHandler => _Container.Resolve<IGetByPropertyValuesHandler<TEntity, TInterface, TId>>();
        public IUpdateHandler<TEntity, TInterface, TId> UpdateHandler => _Container.Resolve<IUpdateHandler<TEntity, TInterface, TId>>();
    }
}
