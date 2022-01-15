using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.DependencyInjection
{

    [TestClass]
    public class EntityClientPerCallModuleTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityList> _MockEntityList;
        private Mock<IExtensionEntityList> _MockExtensionEntityList;
        private Mock<IMappingEntityList> _MockMappingEntityList;

        private static List<string> Entities { get; set; }
            = new List<string> {
                                    "Entity1", "Entity2", "Entity3",
                                    "ExtensionEntity1", "ExtensionEntity2", "ExtensionEntity3",
                                    "MappingEntity1", "MappingEntity2", "MappingEntity3"
                                };
        private static IEnumerable<object[]> TestEntities => Entities.Select(e => new[] { e });

        private static List<string> ExtensionEntities { get; set; }
            = new List<string> { "ExtensionEntity1", "ExtensionEntity2", "ExtensionEntity3" };
        private static IEnumerable<object[]> TestExtensionEntities => ExtensionEntities.Select(e => new[] { e });

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

            _MockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
            _MockExtensionEntityList.Setup(m => m.EntityNames).Returns(ExtensionEntities);

            _MockMappingEntityList = _MockRepository.Create<IMappingEntityList>();
            _MockMappingEntityList.Setup(m => m.EntityNames).Returns(MappingEntities);
            _MockMappingEntityList.Setup(m => m.Entities).Returns(MappingEntityTypes);

            var builder = new ContainerBuilder();
            // Register upstream dependencies
            var entityClientCommonModule = new EntityClientCommonModule(
                            _MockEntityList.Object,
                            _MockMappingEntityList.Object);
            builder.RegisterModule(entityClientCommonModule);

            var mockAppSettings = _MockRepository.Create<IAppSettings>();
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            // Register module to test
            var module = new EntityClientPerCallModule(
                                 _MockEntityList.Object,
                                 _MockExtensionEntityList.Object,
                                 _MockMappingEntityList.Object);

            builder.RegisterModule(module);
            _Container = builder.Build();
        }

        [TestMethod]
        public void EntityClientModule_IEntityClientConfig_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityClientConfig>());
        }

        [TestMethod]
        public void EntityClientModule_IEntityClientConfig_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IEntityClientConfig>();
            var item2 = _Container.Resolve<IEntityClientConfig>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void EntityClientModule_IHeaders_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IHeaders>());
        }

        [TestMethod]
        public void EntityClientModule_IHeaders_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IHeaders>();
            var item2 = _Container.Resolve<IHeaders>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        [DynamicData(nameof(TestEntities))]
        public void EntityClientModule_IEntityClientConnectionSettings_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IEntityClientConnectionSettings>(entity));
        }

        [TestMethod]
        [DynamicData(nameof(TestEntities))]
        public void EntityClientModule_IEntityClientConnectionSettings_IsRegistered_Singleton_Test(string entity)
        {

            var item1 = _Container.ResolveNamed<IEntityClientConnectionSettings>(entity);
            var item2 = _Container.ResolveNamed<IEntityClientConnectionSettings>(entity);
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        [DynamicData(nameof(TestEntities))]
        public void EntityClientModule_IEntityClientAsync_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IEntityClientAsync>(entity));
        }

        [TestMethod]
        [DynamicData(nameof(TestExtensionEntities))]
        public void EntityClientModule_IExtensionEntityClientAsync_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IExtensionEntityClientAsync>(entity));
        }

        [TestMethod]
        [DynamicData(nameof(TestMappingEntities))]
        public void EntityClientModule_IMappingEntityClientAsync_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IMappingEntityClientAsync>(entity));
        }

        [TestMethod]
        public void EntityClientModule_IEntityClientAsyncGeneric1_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityClientAsync<Entity1, int>>());
        }

        [TestMethod]
        public void EntityClientModule_IEntityClientAsyncGeneric2_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityClientAsync<Entity2, long>>());
        }

        [TestMethod]
        public void EntityClientModule_IEntityClientAsyncGeneric3_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IEntityClientAsync<Entity3, string>>());
        }

        [TestMethod]
        public void EntityClientModule_IExtensionEntityClientAsyncGeneric1_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IExtensionEntityClientAsync<ExtensionEntity1, long>>());
        }

        [TestMethod]
        public void EntityClientModule_IExtensionEntityClientAsyncGeneric2_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IExtensionEntityClientAsync<ExtensionEntity2, long>>());
        }

        [TestMethod]
        public void EntityClientModule_IExtensionEntityClientAsyncGeneric3_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IExtensionEntityClientAsync<ExtensionEntity3, long>>());
        }

        [TestMethod]
        public void EntityClientModule_IMappingEntityClientAsyncGeneric1_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IMappingEntityClientAsync<MappingEntity1, int, int, long>>());
        }

        [TestMethod]
        public void EntityClientModule_IMappingEntityClientAsyncGeneric2_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IMappingEntityClientAsync<MappingEntity2, int, int, long>>());
        }

        [TestMethod]
        public void EntityClientModule_IMappingEntityClientAsyncGeneric3_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IMappingEntityClientAsync<MappingEntity3, int, long, string>>());
        }
    }
}