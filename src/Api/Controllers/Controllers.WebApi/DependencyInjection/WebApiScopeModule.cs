using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.WebServices.DependencyInjection;
using Rhyous.Odata.Filter;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;

namespace Rhyous.EntityAnywhere.WebApi
{
    /// <summary>
    /// Autofac registrations for the WCF Scope. Anything that should be in root, or shouldn't be available to the 
    /// initial plugin loaders, should go in here.
    /// </summary>
    public class WebApiScopeModule : Module
    {
        /// <summary>
        /// Registers the modules
        /// </summary>
        /// <param name="builder">The <see cref="ContainerBuilder"/> to register the modules.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c) => new AutofacRuntimePluginLoaderFactory(c.Resolve<ILifetimeScope>()))
                   .As<AutofacRuntimePluginLoaderFactory>()
                   .As<IRuntimePluginLoaderFactory>();
            builder.RegisterInstance(RuntimePluginLoaderFactory.Instance)
                   .As<RuntimePluginLoaderFactory>()
                   .SingleInstance();
            builder.RegisterType<JWTToken>()
                   .As<IJWTToken>()
                   .SingleInstance();
            builder.RegisterType<TokenDecoder>()
                   .As<ITokenDecoder>()
                   .SingleInstance();
            builder.RegisterType<UserRoleEntityDataCache>()
                   .As<IUserRoleEntityDataCache>()
                   .SingleInstance();
            builder.RegisterType<AdminClaimsProvider>()
                   .As<IAdminClaimsProvider>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(FilterExpressionParser<>))
                   .As(typeof(IFilterExpressionParser<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(FilterExpressionParserActionDictionary<>))
                   .As(typeof(IFilterExpressionParserActionDictionary<>))
                   .SingleInstance();

            // This allows these to be registered once, however, see comments in module
            builder.RegisterModule<RestHandlerRegistrationModule>();
            builder.RegisterGeneric(typeof(AlternateKeyTracker<,>))
                   .SingleInstance();

            builder.RegisterType<CachePopulator>()
                   .As<ICachePopulator>()
                   .SingleInstance();
        }
    }
}