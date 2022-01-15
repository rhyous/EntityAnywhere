using Autofac;

namespace Rhyous.EntityAnywhere.HeaderValidators.DependencyInjection
{
    public class TokenHeaderValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<HeaderValidatorsCommonModule>();
            builder.RegisterType<HeadersUpdater>()
                   .As<IHeadersUpdater>()
                   .SingleInstance();
            builder.RegisterType<CustomCustomerRoleAuthorization>()
                   .As<ICustomCustomerRoleAuthorization>()
                   .SingleInstance();
        }
    }
}