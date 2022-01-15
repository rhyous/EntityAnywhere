using Autofac;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.DependencyInjection
{
    public class AuthenticationServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthenticationSettings>().As<IAuthenticationSettings>();
            builder.RegisterType<UserRoleProvider>().As<IUserRoleProvider>();
            builder.RegisterType<AccountLocker>().As<IAccountLocker>();
            builder.RegisterType<PluginCredentialsValidator>().As<ICredentialsValidatorAsync>();
            builder.RegisterType<BasicAuth>().As<IBasicAuth>();
            builder.RegisterType<BasicAuthEncoder>().As<IBasicAuthEncoder>();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
        }
    }
}