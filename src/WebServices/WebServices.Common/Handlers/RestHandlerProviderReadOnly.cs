using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// Since each Endpoint will only use one handler, we shouldn't have Autofac
    /// instantiate all the ones that won't be used. So we will pass this into the 
    /// endpoint and only the necessary object for a specific call will be instantiated.
    /// </summary>
    /// <typeparam name="TEntity">The Entity</typeparam>
    /// <typeparam name="TInterface">The Entity Interface</typeparam>
    /// <typeparam name="TId">The type of the Entity's Id property.</typeparam>
    public class RestHandlerProviderReadOnly<TEntity, TInterface, TId> : IRestHandlerProviderReadOnly<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        protected readonly ILifetimeScope _Container;

        public RestHandlerProviderReadOnly(ILifetimeScope container)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IGetAllHandler<TEntity, TInterface, TId> GetAllHandler => _Container.Resolve<IGetAllHandler<TEntity, TInterface, TId>>();
        public IGetByEntityIdentifiers<TEntity, TInterface, TId> GetByEntityIdentifiersHandler => _Container.Resolve<IGetByEntityIdentifiers<TEntity, TInterface, TId>>();
        public IGetByIdHandler<TEntity, TInterface, TId> GetByIdHandler => _Container.Resolve<IGetByIdHandler<TEntity, TInterface, TId>>();
        public IGetByIdsHandler<TEntity, TInterface, TId> GetByIdsHandler => _Container.Resolve<IGetByIdsHandler<TEntity, TInterface, TId>>();
        public IGetByPropertyValuesHandler<TEntity, TInterface, TId> GetByPropertyValuesHandler => _Container.Resolve<IGetByPropertyValuesHandler<TEntity, TInterface, TId>>();
        public IGetCountHandler<TEntity, TInterface, TId> GetCountHandler => _Container.Resolve<IGetCountHandler<TEntity, TInterface, TId>>();
        public IGetMetadataHandler GetMetadataHandler => _Container.Resolve<IGetMetadataHandler>();
        public IGetPropertyHandler<TEntity, TInterface, TId> GetPropertyHandler => _Container.Resolve<IGetPropertyHandler<TEntity, TInterface, TId>>();
    }
}