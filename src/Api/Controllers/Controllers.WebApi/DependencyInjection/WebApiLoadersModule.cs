using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;
using Rhyous.EntityAnywhere.WebServices;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class WebApiLoadersModule : Module
    {
        private readonly ContainerBuilder _WcfScopeBuilder;

        public WebApiLoadersModule(ContainerBuilder WcfScopeBuilder)
        {
            _WcfScopeBuilder = WcfScopeBuilder;
        }
        protected override void Load(ContainerBuilder builder)
        {
            // WebApi Loaders
            builder.RegisterType<EntityLoader>().As<IEntityLoader>()
                        .SingleInstance();
            builder.RegisterType<CustomWebApiControllerLoader>().As<ICustomWebServiceLoader>()
                        .SingleInstance();
            builder.RegisterType<EntityActionNameAttributeUpdater>().As<IEntityActionNameAttributeUpdater>();
            builder.RegisterType<AttributeToServiceDictionary>().As<IAttributeToServiceDictionary>();
            builder.RegisterType<EntityControllerBuilder>().As<IEntityControllerBuilder>();
            builder.RegisterType<EntityControllerListBuilder>().As<IEntityControllerListBuilder>();
            builder.RegisterType<LoadedEntitiesTracker>().As<ILoadedEntitiesTracker>()
                   .SingleInstance();
            builder.RegisterType<Starter>().As<IStarter>()
                        .SingleInstance();
        }
    }
}