using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RestHandlerProvider<TEntity, TInterface, TId> : RestHandlerProviderReadOnly<TEntity, TInterface, TId>,
                                                                 IRestHandlerProvider<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public RestHandlerProvider(ILifetimeScope container) : base(container)
        {
        }

        public IDeleteHandler<TEntity, TInterface, TId> DeleteHandler => _Container.Resolve<IDeleteHandler<TEntity, TInterface, TId>>();
        public IDeleteManyHandler<TEntity, TInterface, TId> DeleteManyHandler => _Container.Resolve<IDeleteManyHandler<TEntity, TInterface, TId>>();
        public IGenerateRepositoryHandler<TEntity, TInterface, TId> GenerateRepositoryHandler => _Container.Resolve<IGenerateRepositoryHandler<TEntity, TInterface, TId>>();
        public IInsertSeedDataHandler<TEntity, TInterface, TId> InsertSeedDataHandler => _Container.Resolve<IInsertSeedDataHandler<TEntity, TInterface, TId>>();
        public IPatchHandler<TEntity, TInterface, TId> PatchHandler => _Container.Resolve<IPatchHandler<TEntity, TInterface, TId>>();
        public IPostHandler<TEntity, TInterface, TId> PostHandler => _Container.Resolve<IPostHandler<TEntity, TInterface, TId>>();
        public IPutHandler<TEntity, TInterface, TId> PutHandler => _Container.Resolve<IPutHandler<TEntity, TInterface, TId>>();
        public IUpdatePropertyHandler<TEntity, TInterface, TId> UpdatePropertyHandler => _Container.Resolve<IUpdatePropertyHandler<TEntity, TInterface, TId>>();
        public IPostExtensionHandler<TEntity, TInterface, TId> PostExtensionHandler => _Container.Resolve<IPostExtensionHandler<TEntity, TInterface, TId>>();
        public IUpdateExtensionValueHandler<TEntity, TInterface, TId> UpdateExtensionValueHandler => _Container.Resolve<IUpdateExtensionValueHandler<TEntity, TInterface, TId>>();
    }
}