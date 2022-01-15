using Autofac;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    public class AuthenticationClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationSettings>()
                   .As<IAuthenticationSettings>()
                   .SingleInstance();
            builder.RegisterType<AuthenticationClientFactory>()
                   .As<IAuthenticationClientFactory>()
                   .SingleInstance();
            builder.RegisterType<AuthenticationClient>()
                   .As<IAuthenticationClient>()
                   .SingleInstance();
        }
    }
}