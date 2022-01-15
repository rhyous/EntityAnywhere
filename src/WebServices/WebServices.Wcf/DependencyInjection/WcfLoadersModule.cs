using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class WcfLoadersModule : Module
    {
        private readonly ContainerBuilder _WcfScopeBuilder;

        public WcfLoadersModule(ContainerBuilder WcfScopeBuilder)
        {
            _WcfScopeBuilder = WcfScopeBuilder;
        }
        protected override void Load(ContainerBuilder builder)
        {
            // WCF Loaders
            builder.RegisterInstance(new WebServiceRegistrar(_WcfScopeBuilder)).As<IWebServiceRegistrar>();
            builder.RegisterType<EntityLoader>().As<IEntityLoader>()
                        .SingleInstance();
            builder.RegisterType<CustomWebServiceLoader>().As<ICustomWebServiceLoader>()
                        .SingleInstance();
            builder.RegisterType<WebServiceLoaderFactory>().As<IWebServiceLoaderFactory>()
                        .SingleInstance();
            builder.RegisterGeneric(typeof(EntityWebServiceLoader<,>)).As(typeof(IEntityWebServiceLoader<,>))
                        .SingleInstance();
            builder.RegisterType<EntityWebServiceBuilder>().As<IEntityWebServiceBuilder>()
                        .SingleInstance();
            builder.RegisterType<RestEndpointBuilder>().As<IEndpointBuilder>()
                        .SingleInstance();
            builder.RegisterType<Starter>().As<IStarter>()
                        .SingleInstance();
        }
    }
}