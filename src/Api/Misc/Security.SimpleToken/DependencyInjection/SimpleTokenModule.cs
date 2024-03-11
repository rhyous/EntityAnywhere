using Autofac;

namespace Rhyous.EntityAnywhere.Security.DependencyInjection
{
    public class SimpleTokenModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Handlers
            builder.RegisterType<AccessController>().As<IAccessController>()
                   .SingleInstance();
            builder.RegisterType<AnonymousAllowedUrls>().As<IAnonymousAllowedUrls>()
                   .SingleInstance();
            builder.RegisterType<AuthenticationTicketBuilder>().As<IAuthenticationTicketBuilder>()
                   .SingleInstance();
            builder.RegisterType<PluginHeaderValidator>().As<IPluginHeaderValidator>()
                   .SingleInstance();
        }
    }
}