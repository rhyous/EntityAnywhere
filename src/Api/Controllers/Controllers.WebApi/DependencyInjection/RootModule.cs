using Autofac;
using Rhyous.Odata.Expand;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;
using Rhyous.EntityAnywhere.Tools;
using Rhyous.Wrappers;
using IAppSettings = Rhyous.EntityAnywhere.Interfaces.IAppSettings;
using Rhyous.StringLibrary.Pluralization;
using ILogger = Rhyous.EntityAnywhere.Interfaces.ILogger;
using Rhyous.EntityAnywhere.WebServices;

namespace Rhyous.EntityAnywhere.WebApi
{
    public class RootModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(IETFLanguageTagDictionary.Instance)
                   .SingleInstance();
            builder.RegisterType<CustomPluralizer>().As<ICustomPluralizer>()
                   .SingleInstance();
            builder.RegisterType<FileIO>().As<IFileIO>()
                   .SingleInstance();

            // Logger
            builder.RegisterType<LoggerLoader>().As<ILogger>()
                   .SingleInstance();

            // Plugin Loader Module and overrides
            builder.RegisterModule<SimplePluginLoaderModule>();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                builder.RegisterType<AppSettingsContainer>()
                       .As<SimplePluginLoader.IAppSettings>()
                       .As<IAppSettings>().WithParameter("appSettings", System.Configuration.ConfigurationManager.AppSettings);
            }
            else
            {
                builder.RegisterType<EnvironmentVariableAppSettings>()
                       .As<SimplePluginLoader.IAppSettings>()
                       .As<IAppSettings>()
                       .SingleInstance();
            }
            builder.RegisterType<PluginLoaderSettings>()
                   .As<IPluginLoaderSettings>()
                   .SingleInstance();
            builder.RegisterType<MyPluginPaths>().As<IPluginPaths>()
                   .SingleInstance();

            // Application Settings
            builder.RegisterType<TokenKeyPair>().As<ITokenKeyPair>()
                   .SingleInstance();

            // Entity PropertyInfo
            builder.RegisterType<AutofacDIResolver>().As<IDependencyInjectionResolver>()
                   .SingleInstance();
            builder.RegisterGeneric(typeof(TypeInfo<>)).As(typeof(ITypeInfo<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(EntityInfo<>)).As(typeof(IEntityInfo<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(EntityInfoAltKey<,>)).As(typeof(IEntityInfoAltKey<,>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(EntityInfoMapping<,,>)).As(typeof(IEntityInfoMapping<,,>))
                   .SingleInstance();
            builder.RegisterType<TypeInfoResolver>()
                   .As<ITypeInfoResolver>()
                   .SingleInstance();

            builder.RegisterType<PropertyOrderSorter>().As<IPropertyOrderSorter>().SingleInstance();
            builder.RegisterInstance(PreferentialPropertyComparer.Instance).As<IPreferentialPropertyComparer>().SingleInstance();

            // Entity Lists
            builder.RegisterType<EntityList>().As<IEntityList>().SingleInstance();
            builder.RegisterType<ExtensionEntityList>().As<IExtensionEntityList>()
                   .SingleInstance();
            builder.RegisterType<MappingEntityList>().As<IMappingEntityList>()
                   .SingleInstance();

            // Rest Web Service dependencies that are not per call
            builder.RegisterType<AttributeEvaluator>();
            builder.RegisterGeneric(typeof(InputValidator<,>)).As(typeof(IInputValidator<,>));
            builder.RegisterGeneric(typeof(IdDisambiguator<,>)).As(typeof(IIdDisambiguator<,>));
            builder.RegisterGeneric(typeof(IdDisambiguator<,,>)).As(typeof(IIdDisambiguator<,,>));

            // WebApi Configurators Stuff
            builder.RegisterType<AppConfigurator>().As<IAppConfigurator>();
            builder.RegisterType<ContainerConfigurator>().As<IContainerConfigurator>();
            builder.RegisterType<HostConfigurator>().As<IHostConfigurator>();
            builder.RegisterType<ServiceConfigurator>().As<IServiceConfigurator>();

            // WebApi - Objects to create Entity Controllers
            builder.RegisterType<EntityControllerFeatureProvider>().As<IEntityControllerFeatureProvider>().SingleInstance();
            builder.RegisterType<EntityControllerList>().As<IEntityControllerList>().SingleInstance();
        }
    }
}
