using Autofac;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services
{
    public class MetadataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntitySettingsProvider>().As<IEntitySettingsProvider>().SingleInstance();
            builder.RegisterType<MetadataCache>().As<IMetadataCache>().SingleInstance();
            builder.RegisterType<MetadataServiceFactory>().As<IMetadataServiceFactory>().SingleInstance();
        }
    }
}
