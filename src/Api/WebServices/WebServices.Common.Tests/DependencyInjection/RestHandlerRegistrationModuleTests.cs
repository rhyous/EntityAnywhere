using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.EntityAnywhere.WebApi;
using Rhyous.EntityAnywhere.WebServices.DependencyInjection;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.DependencyInjection
{
    [TestClass]
    public class RestHandlerRegistrationModuleTests
    {
        private MockRepository _MockRepository;
        private Mock<IHeaders> _MockHeaders;
        private Mock<IUrlParameters> _MockUrlParameters;
        private Mock<IRequestUri> _MockRequestUri;
        private Mock<IHttpStatusCodeSetter> _MockStatusCodeSetter;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IRelatedEntityProvider<EntityInt, IEntityInt, int>> _MockRelatedEntityProviderEntityint;
        private Mock<IRelatedEntityProvider<MappingEntity1, IMappingEntity1, int>> _MockRelatedEntityProviderMappingEntity1;
        private Mock<IAdminExtensionEntityClientAsync> _MockExtensionEntityClientAsync;
        private Mock<IMetadataCache> _MockMetadataCache;
        private Mock<ILogger> _MockLogger;

        private IContainer _RootContainer;
        private ILifetimeScope _WcfLifetimeScope;
        private ILifetimeScope _PerCallScope;
        private IRestHandlerProvider _RestHandlerProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHeaders = _MockRepository.Create<IHeaders>();
            _MockRequestUri = _MockRepository.Create<IRequestUri>();
            _MockUrlParameters = _MockRepository.Create<IUrlParameters>();
            _MockStatusCodeSetter = _MockRepository.Create<IHttpStatusCodeSetter>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockRelatedEntityProviderEntityint = _MockRepository.Create<IRelatedEntityProvider<EntityInt, IEntityInt, int>>();
            _MockRelatedEntityProviderMappingEntity1 = _MockRepository.Create<IRelatedEntityProvider<MappingEntity1, IMappingEntity1, int>>();
            _MockExtensionEntityClientAsync = _MockRepository.Create<IAdminExtensionEntityClientAsync>();
            _MockMetadataCache = _MockRepository.Create<IMetadataCache>();

            _MockLogger = _MockRepository.Create<ILogger>();

            var rootBuilder = new ContainerBuilder();

            // Register items in root scope
            rootBuilder.RegisterModule<RootModule>();
            rootBuilder.RegisterInstance(_MockAppSettings.Object).As<IAppSettings>();
            rootBuilder.RegisterInstance(_MockLogger.Object).As<ILogger>();

            // Register mocks for Clients
            rootBuilder.RegisterGeneric(typeof(NamedFactory<>)).As(typeof(INamedFactory<>));


            // Some items have downstream dependencies. Even though they are registered here, only a child scope
            // with additional per call registrations can instantiate them as they have dependencies on per call
            // registrations, however, they only need to be registered once, as we don't want the overhead of 
            // registering all of thse items per call
            _RootContainer = rootBuilder.Build();

            _WcfLifetimeScope = _RootContainer.BeginLifetimeScope(b =>
            {
                b.RegisterInstance(_MockHeaders.Object).As<IHeaders>();
                b.RegisterInstance(_MockUrlParameters.Object).As<IUrlParameters>();
                b.RegisterInstance(_MockRequestUri.Object).As<IRequestUri>();
                b.RegisterInstance(_MockStatusCodeSetter.Object).As<IHttpStatusCodeSetter>();
                b.RegisterInstance(_MockRelatedEntityProviderEntityint.Object).As<IRelatedEntityProvider<EntityInt, IEntityInt, int>>();
                b.RegisterInstance(_MockRelatedEntityProviderMappingEntity1.Object).As<IRelatedEntityProvider<MappingEntity1, IMappingEntity1, int>>();
                b.RegisterInstance(_MockExtensionEntityClientAsync.Object).As<IAdminExtensionEntityClientAsync>().Named<IAdminExtensionEntityClientAsync>("AlternateId");
                b.RegisterInstance(_MockMetadataCache.Object).As<IMetadataCache>();
                
                // Register the module
                b.RegisterModule<RestHandlerRegistrationModule>();
            });


            _PerCallScope = _WcfLifetimeScope.BeginLifetimeScope(b =>
            {

                b.RegisterType<RestHandlerProvider>().As<IRestHandlerProvider>();
            });
            _RestHandlerProvider = _PerCallScope.Resolve<IRestHandlerProvider>();
        }

        #region Registration tests

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetMetadataHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetMetadataHandler>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IDeleteHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IDeleteHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IDeleteManyHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IDeleteManyHandler<EntityInt, IEntityInt, int>>());
        }
        
        [TestMethod]
        public void RestHandlerRegistrationModule_IGenerateRepositoryHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGenerateRepositoryHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetAllHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetAllHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByAlternateKeyHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByAlternateKeyHandler<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByEntityIdentifiers_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByEntityIdentifiers<ExtensionEntityBasic, IExtensionEntity, long>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByIdHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByIdHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByIdsHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByIdsHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByMappedIdHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByMappedIdsHandler<MappingEntity1, IMappingEntity1, int, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByMappingsHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByMappingsHandler<MappingEntity1, IMappingEntity1, int, int, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByPropertyValuesHandler_TProp_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByPropertyValuesHandler<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByPropertyValuesHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByPropertyValuesHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByPropertyValuePairsHandler_TProp_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByPropertyValuePairsHandler<ExtensionEntityBasic, IExtensionEntity, long>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByPropertyValuePairsHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetByPropertyValuePairsHandler<ExtensionEntityBasic, IExtensionEntity, long>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetCountHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetCountHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetPropertyHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IGetPropertyHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IInsertSeedDataHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IInsertSeedDataHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPatchHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IPatchHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPostHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IPostHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPostExtensionHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IPostExtensionHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPutHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IPutHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IUpdateExtensionValueHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IUpdateExtensionValueHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IUpdatePropertyHandler_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IUpdatePropertyHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IObjectCreator_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<SimplePluginLoader.IObjectCreator<EntityInt>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRepositoryLoader_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IRepositoryLoader<IRepository<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRepositoryLoaderCommon_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IRepositoryLoaderCommon<IRepository<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRepository_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IRepository<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityServiceLoader_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IEntityServiceLoader<IServiceCommon<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityServiceLoaderCommon_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IEntityServiceLoaderCommon<IServiceCommon<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IServiceCommon_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IServiceCommon<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IServiceCommonAlternateKey_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IServiceCommonAlternateKey<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_EntityEventLoader_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<EntityEventLoader<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityEventLoaderCommon_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IEntityEventLoaderCommon<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityEventAll_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IEntityEventAll<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IDistinctPropertiesEnforcer_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IDistinctPropertiesEnforcer<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRelatedEntityEnforcer_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IRelatedEntityEnforcer<EntityInt>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRelatedEntityRulesBuilder_Registered()
        {
            Assert.IsNotNull(_RestHandlerProvider.Provide<IRelatedEntityRulesBuilder<EntityInt>>());
        }
        #endregion
    }
}