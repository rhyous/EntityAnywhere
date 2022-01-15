using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public class ServiceHandlerProviderAltKey<TEntity, TInterface, TId, TAltKey> : ServiceHandlerProvider<TEntity, TInterface, TId>, IServiceHandlerProviderAltKey<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly ILifetimeScope _Container;

        public ServiceHandlerProviderAltKey(ILifetimeScope container) : base(container)
        {
            _Container = container;
        }

        public IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey> AddAltKeyHandler => _Container.Resolve<IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey>>();
        public IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> GetByAlternateKeyHandler => _Container.Resolve<IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>();
        public ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> SearchByAlternateKeyHandler => _Container.Resolve<ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>();
        public IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey> UpdateAltKeyHandler => _Container.Resolve<IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey>>();
    }
}