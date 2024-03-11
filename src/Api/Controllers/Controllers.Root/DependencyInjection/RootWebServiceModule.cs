using Autofac;

namespace Rhyous.EntityAnywhere.WebServices.DependencyInjection
{
    public class RootWebServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntitySettingsHandler>().As<IEntitySettingsHandler>();
            builder.RegisterType<GenerateHandler>().As<IGenerateHandler>();
            builder.RegisterType<SeedEntityHandler>().As<ISeedEntityHandler>();
            builder.RegisterType<EntityCaller>().As<IEntityCaller>();
            builder.RegisterType<ImpersonationHandler>().As<IImpersonationHandler>();
        }
    }
}