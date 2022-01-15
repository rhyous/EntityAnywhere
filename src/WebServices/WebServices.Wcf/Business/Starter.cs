using Autofac;
using Autofac.Integration.Wcf;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.EntityAnywhere.Clients2.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System.Linq;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This starts EAF.
    /// </summary>
    public class Starter : IStarter
    {
        public static IContainer RootContainer;

        private readonly ICustomWebServiceLoader _CustomWebServiceLoader;
        private readonly IEntityLoader _EntityLoader;
        private readonly ILogger _LoggerLoader;
        private readonly IEntityWebServiceBuilder _EntityWebServiceBuilder;

        public Starter(ICustomWebServiceLoader customWebServiceLoader,
                       IEntityLoader entityLoader,
                       ILogger loggerLoader,
                       IEntityWebServiceBuilder entityWebServiceBuilder)
        {
            _CustomWebServiceLoader = customWebServiceLoader;
            _EntityLoader = entityLoader;
            _LoggerLoader = loggerLoader;
            _EntityWebServiceBuilder = entityWebServiceBuilder;
        }

        public void Initialize()
        {
            _LoggerLoader.Write("Entity Anywhere system started.");
            var entityTypes = _EntityLoader.PluginTypes.OrderBy(pt=>pt.Name).ToList();
            _EntityLoader.Load(entityTypes);
            _CustomWebServiceLoader.Load();
            _EntityWebServiceBuilder.Build(entityTypes);
        }

        /// <summary>
        /// This starts up the custom and common entity services. Custom services are loaded first.
        /// </summary>
        public static void Start()
        {
            // Enable InstancePerContextModules
            AutofacHostFactory.Features |= Features.InstancePerContextModules;

            // Create container
            var rootScope = new ContainerBuilder();
            rootScope.RegisterModule<RootModule>();
            RootContainer = rootScope.Build();

            var pluralizer = RootContainer.Resolve<ICustomPluralizer>();
            PluginLoaderSettings.Default = RootContainer.Resolve<IPluginLoaderSettings>();

            var wcfScope = RootContainer.BeginLifetimeScope("WCFScope", wcfScopeBuilder =>
            {
                using (var innerScope = RootContainer.BeginLifetimeScope(innerBuilder =>
                {
                    innerBuilder.RegisterInstance(wcfScopeBuilder).As<ContainerBuilder>();
                    var wcfLoadersModule = new WcfLoadersModule(wcfScopeBuilder);
                    innerBuilder.RegisterModule(wcfLoadersModule);
                    innerBuilder.RegisterType<LoadedEntitiesTracker>().As<ILoadedEntitiesTracker>()
                                .SingleInstance();
                }))
                {
                    var starter = innerScope.Resolve<IStarter>();
                    starter.Initialize();
                }

                // Load entity client modules
                wcfScopeBuilder.RegisterModule<WcfScopeModule>();
                var entityList = RootContainer.Resolve<IEntityList>();
                var extensionEntityList = RootContainer.Resolve<IExtensionEntityList>();
                var mappingEntityList = RootContainer.Resolve<IMappingEntityList>();
                var entityClientCommonModule = new EntityClientCommonModule(entityList, mappingEntityList);
                wcfScopeBuilder.RegisterModule(entityClientCommonModule);
                var adminEntityClientModule = new AdminEntityClientModule(entityList, extensionEntityList, mappingEntityList);
                wcfScopeBuilder.RegisterModule(adminEntityClientModule);
                wcfScopeBuilder.RegisterModule<MetadataModule>();
            });
            RuntimePluginLoaderFactory.Instance = wcfScope.Resolve<RuntimePluginLoaderFactory>();
            AutofacRuntimePluginLoaderFactory.Instance = wcfScope.Resolve<AutofacRuntimePluginLoaderFactory>();
            AutofacHostFactory.Container = wcfScope;
        }
    }
}