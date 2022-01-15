using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);


            var rootBuilder = new ContainerBuilder();
            // Register mocks of root registrations

            _Container = rootBuilder.Build();


            _WcfScope = _Container.BeginLifetimeScope(b =>
            {
                // Register mocks of WcfScope registrations
                var mockAdminEntityClientAsync = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
                b.RegisterInstance(mockAdminEntityClientAsync.Object).As<IAdminEntityClientAsync<Entity, int>>();

                var mockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
                b.RegisterInstance(mockExtensionEntityList.Object).As<IExtensionEntityList>();

                // Register Module
                b.RegisterModule<MetadataModule>();
            });
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsProvider_Registered()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IEntitySettingsProvider>());
        }

        [TestMethod]
        public void MetadataModule_EntitySettingsProvider_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<IEntitySettingsProvider>(), _WcfScope.Resolve<IEntitySettingsProvider>());
        }

        [TestMethod]
        public void MetadataModule_IMetadataCache_CanResolve()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IMetadataCache>());
        }

        [TestMethod]
        public void MetadataModule_IMetadataCache_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<IMetadataCache>(), _WcfScope.Resolve<IMetadataCache>());
        }

        [TestMethod]
        public void MetadataModule_MetadataServiceFactory_CanResolve()
        {
            Assert.IsNotNull(_WcfScope.Resolve<IMetadataServiceFactory>());
        }

        [TestMethod]
        public void MetadataModule_MetadataServiceFactory_Singleton()
        {
            Assert.AreEqual(_WcfScope.Resolve<IMetadataServiceFactory>(), _WcfScope.Resolve<IMetadataServiceFactory>());
        }
    }
}
