using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2.DependencyInjection;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.DependencyInjection
{

    [TestClass]
    public class AdminEntityClientModuleTests
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
            mockAppSettings.Setup(m => m.Collection).Returns(new NameValueCollection());
            builder.RegisterInstance(mockAppSettings.Object).As<IAppSettings>();

            // Register module to test
            var module = new AdminEntityClientModule(
                           _MockEntityList.Object,
                           _MockExtensionEntityList.Object,
                           _MockMappingEntityList.Object);

            builder.RegisterModule(module);
            _Container = builder.Build();
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminHeaders_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminHeaders>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminHeaders_IsRegistered_Singleton_Test()
        {
            var item1 = _Container.Resolve<IAdminHeaders>();
            var item2 = _Container.Resolve<IAdminHeaders>();
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        [DynamicData(nameof(TestEntities))]
        public void AdminEntityClientModule_IAdminEntityClientAsync_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IAdminEntityClientAsync>(entity));
        }

        [TestMethod]
        [DynamicData(nameof(TestExtensionEntities))]
        public void AdminEntityClientModule_IAdminExtensionEntityClientAsync_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IAdminExtensionEntityClientAsync>(entity));
        }

        [TestMethod]
        [DynamicData(nameof(TestMappingEntities))]
        public void AdminEntityClientModule_IAdminMappingEntityClientAsync_IsRegistered_Test(string entity)
        {
            Assert.IsNotNull(_Container.ResolveNamed<IAdminMappingEntityClientAsync>(entity));
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminEntityClientAsyncGeneric1_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminEntityClientAsync<Entity1, int>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminEntityClientAsyncGeneric2_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminEntityClientAsync<Entity2, long>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminEntityClientAsyncGeneric3_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminEntityClientAsync<Entity3, string>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminExtensionEntityClientAsyncGeneric1_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminExtensionEntityClientAsync<ExtensionEntity1, long>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminExtensionEntityClientAsyncGeneric2_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminExtensionEntityClientAsync<ExtensionEntity2, long>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminExtensionEntityClientAsyncGeneric3_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminExtensionEntityClientAsync<ExtensionEntity3, long>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminMappingEntityClientAsyncGeneric1_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminMappingEntityClientAsync<MappingEntity1, int, int, long>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminMappingEntityClientAsyncGeneric2_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminMappingEntityClientAsync<MappingEntity2, int, int, long>>());
        }

        [TestMethod]
        public void AdminEntityClientModule_IAdminMappingEntityClientAsyncGeneric3_IsRegistered_Test()
        {
            Assert.IsNotNull(_Container.Resolve<IAdminMappingEntityClientAsync<MappingEntity3, int, long, string>>());
        }
    }
}