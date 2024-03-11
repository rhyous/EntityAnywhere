using Autofac;

namespace Rhyous.EntityAnywhere.Services
{
    public class MetadataServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MissingEntitySettingDetector>()
                   .As<IMissingEntitySettingDetector>()
                   .SingleInstance();
            builder.RegisterType<EntitySettingsWriter>()
                   .As<IEntitySettingsWriter>()
                   .SingleInstance();
        }
    }
}