using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests.DependencyInjection
{
    [TestClass]
    public class MetadataModuleTests
    {
        private MockRepository _MockRepository;

        private IContainer _Container;
        private ILifetimeScope _WcfScope;
        private ILifetimeScope _PerCallScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            var rootBuilder = new ContainerBuilder();
            // Register mocks of root registrations
            var mockLogger = _MockRepository.Create<ILogger>();
            rootBuilder.RegisterInstance(mockLogger.Object).As<ILogger>();
            _Container = rootBuilder.Build();


            _WcfScope = _Container.BeginLifetimeScope(b =>
            {
                // Register mocks of WcfScope registrations
                var mockEntityList = _MockRepository.Create<IEntityList>();
                b.RegisterInstance(mockEntityList.Object).As<IEntityList>();

                var mockEntityClientConfig = _MockRepository.Create<IEntityClientConfig>();
                b.RegisterInstance(mockEntityClientConfig.Object).As<IEntityClientConfig>();

                var mockHttpClientRunnerNoHeaders = _MockRepository.Create<IHttpClientRunnerNoHeaders>();
                b.RegisterInstance(mockHttpClientRunnerNoHeaders.Object).As<IHttpClientRunnerNoHeaders>();

                var mockAdminEntityClientAsync = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
                b.RegisterInstance(mockAdminEntityClientAsync.Object).As<IAdminEntityClientAsync<Entity, int>>();

                var mockAdminEntityGroupClientAsync = _MockRepository.Create<IAdminEntityClientAsync<EntityGroup, int>>();
                b.RegisterInstance(mockAdminEntityGroupClientAsync.Object).As<IAdminEntityClientAsync<EntityGroup, int>>();

                var mockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
                b.RegisterInstance(mockExtensionEntityList.Object).As<IExtensionEntityList>();

                // Register Module
                b.RegisterModule<MetadataModule>();
            });

            _PerCallScope = _WcfScope.BeginLifetimeScope(b =>
            {
                // Register mocks of per call registrations
                var mockUrlParameters = _MockRepository.Create<IUrlParameters>();
                b.RegisterInstance(mockUrlParameters.Object).As<IUrlParameters>();
            });
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsCache_Registered()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IEntitySettingsCache>());
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsCache_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IEntitySettingsCache>(), _PerCallScope.Resolve<IEntitySettingsCache>());
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsGroupCache_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IEntityGroupCache>());
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsGroupCache_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IEntityGroupCache>(), _PerCallScope.Resolve<IEntityGroupCache>());
        }

        [TestMethod]
        public void MetadataModule_IMetadataCache_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IMetadataCache>());
        }

        [TestMethod]
        public void MetadataModule_IMetadataCache_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IMetadataCache>(), _PerCallScope.Resolve<IMetadataCache>());
        }


        [TestMethod]
        public void MetadataModule_ICustomMetadataProvider_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ICustomMetadataProvider>());
        }

        [TestMethod]
        public void MetadataModule_ICustomMetadataProvider_NotSingleton()
        {
            Assert.AreNotEqual(_PerCallScope.Resolve<ICustomMetadataProvider>(), _PerCallScope.Resolve<ICustomMetadataProvider>());
        }

        [TestMethod]
        public void MetadataModule_IPropertyFuncProvider_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IPropertyFuncProvider>());
        }

        [TestMethod]
        public void MetadataModule_IPropertyFuncProvider_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IPropertyFuncProvider>(), _PerCallScope.Resolve<IPropertyFuncProvider>());
        }

        [TestMethod]
        public void MetadataModule_IPropertyDataFuncProvider_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IPropertyDataFuncProvider>());
        }

        [TestMethod]
        public void MetadataModule_ICustomPropertyFuncs_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ICustomPropertyFuncs>());
        }

        [TestMethod]
        public void MetadataModule_ICustomPropertyFuncs_NotSingleton()
        {
            Assert.AreNotEqual(_PerCallScope.Resolve<ICustomPropertyFuncs>(), _PerCallScope.Resolve<ICustomPropertyFuncs>());
        }

        [TestMethod]
        public void MetadataModule_IPropertyDataFuncProvider_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IPropertyDataFuncProvider>(), _PerCallScope.Resolve<IPropertyDataFuncProvider>());
        }

        [TestMethod]
        public void MetadataModule_ICustomPropertyDataFuncs_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ICustomPropertyDataFuncs>());
        }

        [TestMethod]
        public void MetadataModule_ICustomPropertyDataFuncs_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<ICustomPropertyDataFuncs>(), _PerCallScope.Resolve<ICustomPropertyDataFuncs>());
        }

        [TestMethod]
        public void MetadataModule_ICsdlBuilderFactory_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<ICsdlBuilderFactory>());
        }

        [TestMethod]
        public void MetadataModule_ICsdlBuilderFactory_CanResolve_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<ICsdlBuilderFactory>(), _PerCallScope.Resolve<ICsdlBuilderFactory>());
        }

        [TestMethod]
        public void MetadataModule_IDisplayNamePropertyFunction_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IDisplayNamePropertyFunction>());
        }

        [TestMethod]
        public void MetadataModule_IDisplayNamePropertyFunction_CanResolve_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IDisplayNamePropertyFunction>(), _PerCallScope.Resolve<IDisplayNamePropertyFunction>());
        }

        [TestMethod]
        public void MetadataModule_IMetadataClient_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IRootClient>());
        }

        [TestMethod]
        public void MetadataModule_IMetadataClient_CanResolve_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IRootClient>(), _PerCallScope.Resolve<IRootClient>());
        }
    }
}