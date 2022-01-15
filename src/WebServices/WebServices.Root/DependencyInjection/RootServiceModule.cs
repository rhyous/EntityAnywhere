using Autofac;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    public class RootServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RootHandlerProvider>().As<IRootHandlerProvider>();
            builder.RegisterType<EntitySettingsHandler>().As<IEntitySettingsHandler>();
            builder.RegisterType<GenerateHandler>().As<IGenerateHandler>();
            builder.RegisterType<SeedEntityHandler>().As<ISeedEntityHandler>();
            builder.RegisterType<EntityCaller>().As<IEntityCaller>();
        }
    }
}