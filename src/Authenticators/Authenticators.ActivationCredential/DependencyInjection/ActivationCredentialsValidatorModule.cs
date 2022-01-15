using Autofac;
using Rhyous.WebFramework.Services.Security.DependencyInjection;

namespace Rhyous.WebFramework.Authenticators.DependencyInjection
{
    public class ActivationCredentialsValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<SecurityModule>();
            builder.RegisterType<ClaimsBuilder>().As<IClaimsBuilder>();
        }
    }
}