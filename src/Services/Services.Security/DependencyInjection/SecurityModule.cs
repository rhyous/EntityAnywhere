using Autofac;

namespace Rhyous.WebFramework.Services.Security.DependencyInjection
{
    /// <summary>
    /// Responsible for registering the necessary dependencies for this service
    /// </summary>
    public class SecurityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PasswordSecuritySettings>().As<IPasswordSecuritySettings>().SingleInstance();
            builder.RegisterType<RijndaelManagedSecurity>().As<IPasswordSecurity>().SingleInstance();
        }
    }
}