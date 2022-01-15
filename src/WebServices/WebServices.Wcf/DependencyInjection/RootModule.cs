using Autofac;
using Rhyous.Odata.Expand;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using Rhyous.EntityAnywhere.PluginLoaders;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Tools;
using Rhyous.Wrappers;
using System.Configuration;
using System.ServiceModel.Description;
using System.Web.Routing;
using IAppSettings = Rhyous.EntityAnywhere.Interfaces.IAppSettings;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class RootModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomPluralizer>().As<ICustomPluralizer>()
                   .SingleInstance();
            builder.RegisterType<FileIO>().As<IFileIO>()
                   .SingleInstance();
            builder.RegisterGeneric(typeof(PreventSimultaneousFuncCalls<>)).As(typeof(IPreventSimultaneousFuncCalls<>));

            // Logger
            builder.RegisterType<LoggerLoader>().As<ILogger>()
                   .SingleInstance();

            // Application Settings
            builder.RegisterType<AppSettingsContainer>().As<IAppSettings>()
                   .WithParameter("appSettings", ConfigurationManager.AppSettings)
                   .SingleInstance();
            builder.RegisterType<TokenKeyPair>().As<ITokenKeyPair>()
                   .SingleInstance();

            // Plugin Loader Module and overrides
            builder.RegisterModule<SimplePluginLoaderModule>();
            builder.RegisterType<MyPluginPaths>().As<IPluginPaths>()
                   .SingleInstance();

            // Entity PropertyInfo
            builder.RegisterGeneric(typeof(EntityInfo<>)).As(typeof(IEntityInfo<>))
                   .SingleInstance();
            builder.RegisterGeneric(typeof(EntityInfoAltKey<,>)).As(typeof(IEntityInfoAltKey<,>))
                   .SingleInstance();

            builder.RegisterType<PropertyOrderSorter>().As<IPropertyOrderSorter>().SingleInstance();
            builder.RegisterType<PreferentialPropertyComparer>().As<IPreferentialPropertyComparer>().SingleInstance();

            // WCF stuff
            builder.RegisterInstance(RouteTable.Routes).As<RouteCollection>();
            builder.RegisterType<AttributeToServiceDictionary>()
                   .SingleInstance();

            builder.RegisterType<ServiceBehaviorLoader>().As<IRuntimePluginLoader<IServiceBehavior>>()
                   .SingleInstance();

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
        }
    }
}