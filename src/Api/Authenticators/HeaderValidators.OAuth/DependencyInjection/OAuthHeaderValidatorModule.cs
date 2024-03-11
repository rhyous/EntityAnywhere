using Autofac;

namespace Rhyous.EntityAnywhere.HeaderValidators.DependencyInjection
{
    public class OAuthHeaderValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<HeaderValidatorsCommonModule>();
            builder.RegisterType<BearerDecoder>()
                   .As<IBearerDecoder>()
                   .SingleInstance();
            builder.RegisterType<HeadersUpdater>()
                   .As<IHeadersUpdater>()
                   .SingleInstance();
            builder.RegisterType<CustomCustomerRoleAuthorization>()
                   .As<ICustomCustomerRoleAuthorization>()
                   .SingleInstance();
            builder.RegisterType<JWTValidator>()
                   .As<IJWTValidator>()
                   .SingleInstance();
            builder.RegisterType<TokenFromClaimsBuilder>()
                   .As<ITokenFromClaimsBuilder>()
                   .SingleInstance();
        }
    }
}