using Autofac;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class TokenModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JWTToken>().As<IJWTToken>();
            builder.RegisterType<ClaimsBuilderAsync>()
                   .As<IClaimsBuilderAsync>();
            builder.RegisterType<TokenGenerator>()
                   .As<ITokenBuilder<IUser>>();
        }
    }
}