using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Clients2.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.DependencyInjection
{

    [TestClass]
    public class EntityClientCommonModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityList> _MockEntityList;
        private Mock<IMappingEntityList> _MockMappingEntityList;

        private static List<string> Entities { get; set; } 
            = new List<string> {
                                    "Entity1", "Entity2", "Entity3", 
                                    "ExtensionEntity1", "ExtensionEntity2", "ExtensionEntity3", 
                                    "MappingEntity1", "MappingEntity2", "MappingEntity3" 
                                };
        private static IEnumerable<object[]> TestEntities => Entities.Select(e => new[] { e });


        private static List<string> MappingEntities { get; set; } 
            = new List<string> { "MappingEntity1", "MappingEntity2", "MappingEntity3" };
        private static IEnumerable<object[]> TestMappingEntities => MappingEntities.Select(e => new[] { e });
        private static List<Type> MappingEntityTypes { get; set; }
            = new List<Type> { typeof(MappingEntity1), typeof(MappingEntity2), typeof(MappingEntity3) };

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityList = _MockRepository.Create<IEntityList>();
            _MockEntityList.Setup(m => m.EntityNames).Returns(Entities);

            _MockMappingEntityList = _MockRepository.Create<IMappingEntityList>();
            _MockMappingEntityList.Setup(m => m.EntityNames).Returns(MappingEntities);
            _MockMappingEntityList.Setup(m => m.Entities).Returns(MappingEntityTypes);

            var builder = new ContainerBuilder();
            // Register upstream dependencies
            var mockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            // Register module to test
            var entityClientCommonModule = new EntityClientCommonModule(
                            _MockEntityList.Object,
                            _MockMappingEntityList.Object);
            builder.RegisterModule(entityClientCommonModule);

            _Container = builder.Build();
        }

        [TestMethod]
        public void EntityClientCommonModule_IHttpClientFactory_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IHttpClientFactory>());
        }

        [TestMethod]
        public void EntityClientCommonModule_IHttpClientFactory_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IHttpClientFactory>();
            var item2 = _Container.Resolve<IHttpClientFactory>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void EntityClientCommonModule_IEntityClientConfig_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityClientConfig>());
        }

        [TestMethod]
        public void EntityClientCommonModule_IEntityClientConfig_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IEntityClientConfig>();
            var item2 = _Container.Resolve<IEntityClientConfig>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void EntityClientCommonModule_INamedFactory_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<INamedFactory<object>>());
        }

        [TestMethod]
        public void EntityClientCommonModule_INamedFactory_IsRegistered_NotASingleton_Test()
        {
            var item1 = _Container.Resolve<INamedFactory<object>>();
            var item2 = _Container.Resolve<INamedFactory<object>>();
            Assert.AreNotEqual(item1, item2);
        }

        [TestMethod]
        public void EntityClientCommonModule_INamedFactory_IsRegistered_NotASingleton_SubScope_Test()
        {
            var subScope = _Container.BeginLifetimeScope();
            var item2 = subScope.Resolve<INamedFactory<object>>();
            Assert.AreEqual(subScope, item2.GetFieldValue("_Container"));
        }

        [TestMethod]
        [DynamicData(nameof(TestEntities))]
        public void EntityClientCommonModule_IEntityClientConnectionSettings_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IEntityClientConnectionSettings>(entity));
        }

        [TestMethod]
        [DynamicData(nameof(TestEntities))]
        public void EntityClientCommonModule_IEntityClientConnectionSettings_IsRegistered_Singleton_Test(string entity)
        {

            var item1 = _Container.ResolveNamed<IEntityClientConnectionSettings>(entity);
            var item2 = _Container.ResolveNamed<IEntityClientConnectionSettings>(entity);
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void EntityClientCommonModule_IEntityClientConnectionSettingsGeneric_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityClientConnectionSettings<Entity1>>());
        }

        [TestMethod]
        public void EntityClientCommonModule_IEntityClientConnectionSettingsGeneric_IsRegistered_Singleton_Test()
        {

            var item1 = _Container.Resolve<IEntityClientConnectionSettings<Entity3>>();
            var item2 = _Container.Resolve<IEntityClientConnectionSettings<Entity3>>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void EntityClientCommonModule_IMappingEntitySettingsGeneric_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IMappingEntitySettings<MappingEntity1>>());
        }

        [TestMethod]
        public void EntityClientCommonModule_IMappingEntitySettingsGeneric_IsRegistered_Singleton_Test()
        {

            var item1 = _Container.Resolve<IMappingEntitySettings<MappingEntity3>>();
            var item2 = _Container.Resolve<IMappingEntitySettings<MappingEntity3>>();
            Assert.AreEqual(item1, item2);
        }
    }
}