using Autofac;

namespace Rhyous.EntityAnywhere.Behaviors.DependencyInjection
{
    public class PluginHeaderValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HeaderValidationInspector>()
                   .As<IHeaderValidationInspector>();
            builder.RegisterType<PluginHeaderValidator>()
                   .As<IPluginHeaderValidator>();
            builder.RegisterType<AccessController>()
                   .As<IAccessController>();
            builder.RegisterType<AnonymousAllowedUrls>()
                   .As<IAnonymousAllowedUrls>();
        }
    }
}