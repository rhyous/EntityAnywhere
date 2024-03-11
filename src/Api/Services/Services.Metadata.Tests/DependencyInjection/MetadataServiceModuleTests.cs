using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests.DependencyInjection
{
    [TestClass]
    public class MetadataServiceModuleTests
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

            var mockEntityInfoResolver = _MockRepository.Create<ITypeInfoResolver>();
            rootBuilder.RegisterInstance(mockEntityInfoResolver.Object).As<ITypeInfoResolver>();

            _Container = rootBuilder.Build();


            _WcfScope = _Container.BeginLifetimeScope(b =>
            {
                // Register mocks of WcfScope registrations
                var mockAdminEntityClientAsync = _MockRepository.Create<IAdminEntityClientAsync<Entity, int>>();
                b.RegisterInstance(mockAdminEntityClientAsync.Object).As<IAdminEntityClientAsync<Entity, int>>();

                var mockAdminEntityPropertyClientAsync = _MockRepository.Create<IAdminEntityClientAsync<EntityProperty, int>>();
                b.RegisterInstance(mockAdminEntityPropertyClientAsync.Object).As<IAdminEntityClientAsync<EntityProperty, int>>();

                var mockAdminEntityGroupClientAsync = _MockRepository.Create<IAdminEntityClientAsync<EntityGroup, int>>();
                b.RegisterInstance(mockAdminEntityGroupClientAsync.Object).As<IAdminEntityClientAsync<EntityGroup, int>>();

                var mockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
                b.RegisterInstance(mockExtensionEntityList.Object).As<IExtensionEntityList>();

                var mockEntitySettingsCache = _MockRepository.Create<IEntitySettingsCache>();
                b.RegisterInstance(mockEntitySettingsCache.Object).As<IEntitySettingsCache>();

                var mockEntityGroupCache = _MockRepository.Create<IEntityGroupCache>();
                b.RegisterInstance(mockEntityGroupCache.Object).As<IEntityGroupCache>();

                // Register Module
                b.RegisterModule<MetadataServiceModule>();
            });

            _PerCallScope = _WcfScope.BeginLifetimeScope(b =>
            {
                // Register mocks of per call registrations
                var mockUrlParameters = _MockRepository.Create<IUrlParameters>();
                b.RegisterInstance(mockUrlParameters.Object).As<IUrlParameters>();
            });
        }

        [TestMethod]
        public void MetadataModule_IMissingEntitySettingDetector_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IMissingEntitySettingDetector>());
        }

        [TestMethod]
        public void MetadataModule_IMissingEntitySettingDetector_CanResolve_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IMissingEntitySettingDetector>(), _PerCallScope.Resolve<IMissingEntitySettingDetector>());
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsWriter_CanResolve()
        {
            Assert.IsNotNull(_PerCallScope.Resolve<IEntitySettingsWriter>());
        }

        [TestMethod]
        public void MetadataModule_IEntitySettingsWriter_CanResolve_Singleton()
        {
            Assert.AreEqual(_PerCallScope.Resolve<IEntitySettingsWriter>(), _PerCallScope.Resolve<IEntitySettingsWriter>());
        }
    }
}
