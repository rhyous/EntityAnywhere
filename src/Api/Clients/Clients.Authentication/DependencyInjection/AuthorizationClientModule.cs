using Autofac;

namespace Rhyous.EntityAnywhere.Clients2.DependencyInjection
{
    public class AuthorizationClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthorizationClientFactory>()
                   .As<IAuthorizationClientFactory>()
                   .SingleInstance();
            builder.RegisterType<AuthorizationClient>()
                   .As<IAuthorizationClient>()
                   .SingleInstance();
        }
    }
}