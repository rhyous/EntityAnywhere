using Autofac;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Cache;
using Rhyous.EntityAnywhere.Clients2;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class MetadataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntitySettingsCache>()
                   .As<IEntitySettingsCache>()
                   .SingleInstance();
            builder.RegisterType<EntityGroupCache>()
                   .As<IEntityGroupCache>()
                   .SingleInstance();
            builder.RegisterType<MetadataCache>()
                   .As<IMetadataCache>()
                   .SingleInstance();
            builder.RegisterType<CustomMetadataProvider>()
                   .As<ICustomMetadataProvider>();
            builder.RegisterType<PropertyFuncProvider>()
                   .As<IPropertyFuncProvider>()
                   .SingleInstance();
            builder.Register(ctx => ctx.Resolve<IPropertyFuncProvider>().Provide())
                   .As<ICustomPropertyFuncs>();
            builder.RegisterType<PropertyDataFuncProvider>()
                   .As<IPropertyDataFuncProvider>()
                   .SingleInstance();
            builder.Register(ctx => ctx.Resolve<IPropertyDataFuncProvider>().Provide())
                   .As<ICustomPropertyDataFuncs>()
                   .SingleInstance();
            builder.RegisterInstance(CsdlBuilderFactory.Instance)
                   .As<ICsdlBuilderFactory>()
                   .SingleInstance();
            builder.RegisterType<DisplayNamePropertyFunction>()
                   .As<IDisplayNamePropertyFunction>()
                   .SingleInstance();
            builder.RegisterType<RootClient>()
                   .As<IRootClient>()
                   .SingleInstance();
        }
    }
}
