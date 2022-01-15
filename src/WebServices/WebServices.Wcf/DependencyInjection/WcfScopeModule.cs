using Autofac;
using Autofac.Integration.Wcf;
using Rhyous.Odata;
using Rhyous.Odata.Filter;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// Autofac registrations for the WCF Scope. Anything that should be in root, or shouldn't be available to the 
    /// initial plugin loaders, should go in here.
    /// </summary>
    public class WcfScopeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PerInstanceContextModuleAccessor>().As<IPerInstanceContextModuleAccessor>();

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
            builder.RegisterType<UserRoleEntityDataCacheFactory>()
                   .As<IUserRoleEntityDataCacheFactory>()
                   .SingleInstance();
            builder.Register(ctx => ctx.Resolve<IUserRoleEntityDataCacheFactory>().Cache)
                   .As<IUserRoleEntityDataCache>()
                   .InstancePerDependency(); // Even though it is instance per dependency, it will behave as a singleton mostly using the IUserRoleEntityDataCacheFactory.
                                             // However, the IUserRoleEntityDataCacheFactory.Cache can be cleared so it can be recreated on edit.
            builder.RegisterType<AdminClaimsProvider>()
                   .As<IAdminClaimsProvider>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(FilterExpressionParser<>))
                   .As(typeof(IFilterExpressionParser<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(FilterExpressionParserActionDictionary<>))
                   .As(typeof(IFilterExpressionParserActionDictionary<>))
                   .SingleInstance();
        }
    }
}