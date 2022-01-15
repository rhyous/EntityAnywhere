using Autofac;

namespace Rhyous.EntityAnywhere.HeaderValidators.DependencyInjection
{
    public class HeaderValidatorsCommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TokenSecurityList>()
                   .As<ITokenSecurityList>()
                   .SingleInstance();
            builder.RegisterType<PathNormalizer>()
                   .As<IPathNormalizer>()
                   .SingleInstance();
            builder.RegisterType<EntityNameProvider>()
                   .As<IEntityNameProvider>()
                   .SingleInstance();
            builder.RegisterType<EntityPermissionChecker>()
                   .As<IEntityPermissionChecker>()
                   .SingleInstance();
        }
    }
}