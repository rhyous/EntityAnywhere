using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey>
                 : RestHandlerProvider<TEntity, TInterface, TId>,
                   IRestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        public RestHandlerProviderAlternateKey(ILifetimeScope container) : base(container.BeginLifetimeScope(b =>
            {
                var service = container.Resolve<IServiceCommon<TEntity, TInterface, TId>>();

                if (!(service is IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey>))
                {
                    b.RegisterType<ServiceProxyAlternateKey<TEntity, TInterface, TId, TAltKey>>()
                        .As<IServiceCommon<TEntity, TInterface, TId>>();
                }
            }))
        {
        }

        public IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> GetByAlternateKeyHandler 
               => _Container.Resolve<IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>>();
    }
}