using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.PluginLoaders;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using Rhyous.EntityAnywhere.WebServices.DependencyInjection;
using Rhyous.Wrappers;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.DependencyInjection
{
    [TestClass]
    public class RestHandlerRegistrationModuleTests
    {
        private MockRepository _MockRepository;
        private Mock<IHeaders> _MockHeaders;
        private Mock<IUrlParameters> _MockUrlParameters;
        private Mock<IRequestUri> _MockRequestUri;
        private Mock<IWebOperationContext> _MockWebOperationContext;
        private Mock<IOutgoingWebResponseContext> _MockOutgoingWebResponseContext;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IRelatedEntityProvider<EntityInt, IEntityInt, int>> _MockRelatedEntityProvider;
        private Mock<IExtensionEntityClientAsync> _MockExtensionEntityClientAsync;
        private Mock<IMetadataServiceFactory> _MockMetadataServiceFactory;
        private Mock<ILogger> _MockLogger;

        private IContainer _RootContainer;
        private ILifetimeScope _WcfLifetimeScope;
        private ILifetimeScope _PerCallScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHeaders = _MockRepository.Create<IHeaders>();
            _MockRequestUri = _MockRepository.Create<IRequestUri>();
            _MockUrlParameters = _MockRepository.Create<IUrlParameters>();
            _MockWebOperationContext = _MockRepository.Create<IWebOperationContext>();
            _MockOutgoingWebResponseContext = _MockRepository.Create<IOutgoingWebResponseContext>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<EntityInt, IEntityInt, int>>();
            _MockExtensionEntityClientAsync = _MockRepository.Create<IExtensionEntityClientAsync>();
            _MockMetadataServiceFactory = _MockRepository.Create<IMetadataServiceFactory>();

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
                b.RegisterInstance(_MockWebOperationContext.Object).As<IWebOperationContext>();
                b.RegisterInstance(_MockOutgoingWebResponseContext.Object).As<IOutgoingWebResponseContext>();
                b.RegisterInstance(_MockRelatedEntityProvider.Object).As<IRelatedEntityProvider<EntityInt, IEntityInt, int>>();
                b.RegisterInstance(_MockExtensionEntityClientAsync.Object).As<IExtensionEntityClientAsync>().Named<IExtensionEntityClientAsync>("AlternateId");
                b.RegisterInstance(_MockMetadataServiceFactory.Object).As<IMetadataServiceFactory>();
            });


            // Register the module
            _PerCallScope = _WcfLifetimeScope.BeginLifetimeScope(b =>
            {
                b.RegisterModule<RestHandlerRegistrationModule>();
            });
        }

        #region Registration tests
        [TestMethod]
        public void RestHandlerRegistrationModule_IRestHandlerProviderReadOnly_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRestHandlerProviderReadOnly<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRestHandlerProvider_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRestHandlerProvider<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRestHandlerProviderAlternateKey_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRestHandlerProviderAlternateKey<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetMetadataHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetMetadataHandler>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IDeleteHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IDeleteHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IDeleteManyHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IDeleteManyHandler<EntityInt, IEntityInt, int>>());
        }
        
        [TestMethod]
        public void RestHandlerRegistrationModule_IGenerateRepositoryHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGenerateRepositoryHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetAllHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetAllHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByAlternateKeyHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByAlternateKeyHandler<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByEntityIdentifiers_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByEntityIdentifiers<ExtensionEntityBasic, IExtensionEntity, long>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByIdHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByIdHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByIdsHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByIdsHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByPropertyValuesHandler_TProp_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByPropertyValuesHandler<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetByPropertyValuesHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetByPropertyValuesHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetCountHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetCountHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IGetPropertyHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IGetPropertyHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IInsertSeedDataHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IInsertSeedDataHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPatchHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IPatchHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPostHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IPostHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPostExtensionHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IPostExtensionHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IPutHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IPutHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IUpdateExtensionValueHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IUpdateExtensionValueHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IUpdatePropertyHandler_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IUpdatePropertyHandler<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IObjectCreator_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<SimplePluginLoader.IObjectCreator<EntityInt>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRepositoryLoader_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRepositoryLoader<IRepository<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRepositoryLoaderCommon_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRepositoryLoaderCommon<IRepository<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRepository_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRepository<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityServiceLoader_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IEntityServiceLoader<IServiceCommon<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityServiceLoaderCommon_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IEntityServiceLoaderCommon<IServiceCommon<EntityInt, IEntityInt, int>, EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IServiceCommon_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IServiceCommon<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IServiceCommonAlternateKey_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IServiceCommonAlternateKey<EntityInt, IEntityInt, int, string>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_EntityEventLoader_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<EntityEventLoader<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IEntityEventAll_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IEntityEventAll<EntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IDistinctPropertiesEnforcer_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IDistinctPropertiesEnforcer<EntityInt, IEntityInt, int>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRelatedEntityEnforcer_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityEnforcer<EntityInt>>());
        }

        [TestMethod]
        public void RestHandlerRegistrationModule_IRelatedEntityRulesBuilder_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRelatedEntityRulesBuilder<EntityInt>>());
        }
        #endregion
    }
}