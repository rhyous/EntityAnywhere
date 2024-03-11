using Autofac;
using Rhyous.EntityAnywhere.Clients2.DependencyInjection;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Security.DependencyInjection;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.SimplePluginLoader;
using Rhyous.SimplePluginLoader.DependencyInjection;
using Rhyous.StringLibrary.Pluralization;
using ILogger = Rhyous.EntityAnywhere.Interfaces.ILogger;

namespace Rhyous.EntityAnywhere.WebApi
{
    /// <summary>
    /// This starts EAF.
    /// </summary>
    public class Starter : IStarter
    {
        public static IContainer? RootContainer;
        public static ILifetimeScope? WebApiScope;

        private readonly IEntityLoader _EntityLoader;
        private readonly ICustomWebServiceLoader _CustomWebServiceLoader;
        private readonly IEntityControllerListBuilder _EntityControllerListBuilder;
        private readonly ILogger _LoggerLoader;

        public Starter(IEntityLoader entityLoader,
                       ICustomWebServiceLoader customWebServiceLoader,
                       IEntityControllerListBuilder entityControllerListBuilder,
                       ILogger loggerLoader)
        {
            _EntityLoader = entityLoader;
            _CustomWebServiceLoader = customWebServiceLoader;
            _EntityControllerListBuilder = entityControllerListBuilder;
            _LoggerLoader = loggerLoader;
        }

        public void Initialize()
        {
            _LoggerLoader.Write("Entity Anywhere system started.");
            var coreEntityTypes = new List<Type> { typeof(Entity), typeof(EntityProperty), typeof(EntityGroup) };
            var pluginEntityTypes = _EntityLoader.PluginTypes;
            var entityTypes = coreEntityTypes.Concat(pluginEntityTypes).OrderBy(pt => pt.Name);
            _EntityLoader.Load(entityTypes);
            _CustomWebServiceLoader.Load();
            _EntityControllerListBuilder.Build(entityTypes);
        }

        /// <summary>
        /// This starts up the custom and common entity services. Custom services are loaded first.
        /// </summary>
        public static void Start()
        {
            // Create container
            var rootScope = new ContainerBuilder();
            rootScope.RegisterModule<RootModule>();
            RootContainer = rootScope.Build();
            IETFLanguageTagDictionary.Instance[""] = IETFLanguageTagDictionary.Instance["en"];
            var pluralizer = RootContainer.Resolve<ICustomPluralizer>();
            PluginLoaderSettings.Default = RootContainer.Resolve<IPluginLoaderSettings>();

            WebApiScope = RootContainer.BeginLifetimeScope("WebApiScope", webApiScopeBuilder =>
            {
                using (var innerScope = RootContainer.BeginLifetimeScope(innerBuilder =>
                {
                    var webApiScopeContainerBuilder = new WebApiScopeContainerBuilder(webApiScopeBuilder);
                    innerBuilder.RegisterInstance(webApiScopeContainerBuilder).As<IWebApiScopeContainerBuilder>();
                    var webApiLoadersModule = new WebApiLoadersModule(webApiScopeBuilder);
                    innerBuilder.RegisterModule(webApiLoadersModule);
                }))
                {
                    var starter = innerScope.Resolve<IStarter>();
                    starter.Initialize();
                }

                // Load entity client modules
                webApiScopeBuilder.RegisterModule<WebApiScopeModule>();
                var entityList = RootContainer.Resolve<IEntityList>();
                var extensionEntityList = RootContainer.Resolve<IExtensionEntityList>();
                var mappingEntityList = RootContainer.Resolve<IMappingEntityList>();
                var entityClientCommonModule = new EntityClientCommonModule(entityList, mappingEntityList);
                webApiScopeBuilder.RegisterModule(entityClientCommonModule);
                var adminEntityClientModule = new AdminEntityClientModule(entityList, extensionEntityList, mappingEntityList);
                webApiScopeBuilder.RegisterModule(adminEntityClientModule);
                webApiScopeBuilder.RegisterModule<MetadataModule>();
                webApiScopeBuilder.RegisterModule<WebApiPerRequestModule>();
                var entityClientPerCallModuleWebApi = new EntityClientPerCallModuleWebApi(entityList, extensionEntityList, mappingEntityList);
                webApiScopeBuilder.RegisterModule(entityClientPerCallModuleWebApi);
                webApiScopeBuilder.RegisterModule<RelatedEntityRegistrationModuleWebApi>();
                webApiScopeBuilder.RegisterModule<SimpleTokenModule>();
            });
            RuntimePluginLoaderFactory.Instance = WebApiScope.Resolve<RuntimePluginLoaderFactory>();
            AutofacRuntimePluginLoaderFactory.Instance = WebApiScope.Resolve<AutofacRuntimePluginLoaderFactory>();
        }
    }
}
