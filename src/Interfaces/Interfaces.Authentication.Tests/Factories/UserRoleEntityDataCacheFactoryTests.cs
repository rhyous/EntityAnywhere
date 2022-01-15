using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Factories
{
    [TestClass]
    public class UserRoleEntityDataCacheFactoryTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IAdminEntityClientAsync>> _MockNamedFactory;
        private Mock<IAdminEntityClientAsync> _MockUserRoleClient;
        private Mock<IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>>> _MockPreventSimultaneousFuncCalls;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminEntityClientAsync>>();
            _MockUserRoleClient = _MockRepository.Create<IAdminEntityClientAsync>();
            _MockPreventSimultaneousFuncCalls = _MockRepository.Create<IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>>>();
        }

    private IUserRoleEntityDataCacheFactory CreateFactory()
        {
            return new UserRoleEntityDataCacheFactory(_MockNamedFactory.Object,
                                                      _MockPreventSimultaneousFuncCalls.Object);
        }

        #region CreateAsync
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UserRoleEntityDataCacheFactory_CreateAsync_UserRoleClient_ReturnsNullEmptyOrWhitespace_CacheIsEmpty(string json)
        {
            // Arrange
            var factory = CreateFactory() as UserRoleEntityDataCacheFactory;
            _MockNamedFactory.Setup(m => m.Create("UserRole"))
                             .Returns(_MockUserRoleClient.Object);
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCacheFactory.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.CreateAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCacheFactory_CreateAsync_UserRoleClient_ReturnsHtmlError_CacheIsEmpty()
        {
            // Arrange
            var factory = CreateFactory() as UserRoleEntityDataCacheFactory;
            _MockNamedFactory.Setup(m => m.Create("UserRole"))
                             .Returns(_MockUserRoleClient.Object);
            var notJson = "<html><body><p>Some html error</p></body></html>";
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCacheFactory.Expand, true))
                               .ReturnsAsync(notJson);

            // Act
            var result = await factory.CreateAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCacheFactory_CreateAsync_Admin_All()
        {
            // Arrange
            var factory = CreateFactory() as UserRoleEntityDataCacheFactory;
            _MockNamedFactory.Setup(m => m.Create(nameof(UserRole)))
                             .Returns(_MockUserRoleClient.Object);
            var userRole = new UserRole { Id = WellknownUserRoleIds.Admin, Name = WellknownUserRole.Admin };
            var odataObjectCollection = new[] { userRole }.AsOdata<UserRole, int>();
            var relatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            odataObjectCollection[0].RelatedEntityCollection.Add(relatedEntityCollection);
            var entity = new Entity { Id = 0, Name = "All" };
            var odataEntity = entity.AsOdata<Entity, int>();
            relatedEntityCollection.Add(odataEntity);
            var json = JsonConvert.SerializeObject(odataObjectCollection);
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCacheFactory.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.CreateAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            var userRoleEntityData = result[1];
            Assert.AreEqual(1, userRoleEntityData.Count);
            Assert.AreEqual("All", userRoleEntityData["All"].Entity);
            Assert.AreEqual(Permissions.Admin, userRoleEntityData["All"].Permissions.First());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCacheFactory_CreateAsync_TestUserRole_HasNoEntityAssignedYet_Test()
        {
            // Arrange
            var factory = CreateFactory() as UserRoleEntityDataCacheFactory;
            _MockNamedFactory.Setup(m => m.Create(nameof(UserRole)))
                             .Returns(_MockUserRoleClient.Object);

            var adminUserRole = new UserRole { Id = 1, Name = WellknownUserRole.Admin };
            var testUserRole = new UserRole { Id = 4, Name = "Test Role" };

            var odataObjectCollection = new[] { adminUserRole, testUserRole }.AsOdata<UserRole, int>();

            var adminRelatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            var entity = new Entity { Id = 0, Name = "All" };
            var odataEntity = entity.AsOdata<Entity, int>();
            adminRelatedEntityCollection.Add(odataEntity);
            odataObjectCollection[0].RelatedEntityCollection.Add(adminRelatedEntityCollection);

            var entity2 = new Entity { Id = 32, Name = "ProductGroup" };
            var odataEntity2 = entity2.AsOdata<Entity, int>();

            var json = JsonConvert.SerializeObject(odataObjectCollection);
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCacheFactory.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.CreateAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            var adminRoleEntityData = result[1];
            Assert.AreEqual(1, adminRoleEntityData.Count);
            var adminPermission = adminRoleEntityData["All"];
            Assert.AreEqual("All", adminPermission.Entity);
            Assert.AreEqual(Permissions.Admin, adminPermission.Permissions.First());

            var testRoleEntityData = result[4];
            Assert.AreEqual(0, testRoleEntityData.Count);

            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCacheFactory_CreateAsync_Admin_SpecifiedEntities_Test()
        {
            // Arrange
            var factory = CreateFactory() as UserRoleEntityDataCacheFactory;
            _MockNamedFactory.Setup(m => m.Create(nameof(UserRole)))
                             .Returns(_MockUserRoleClient.Object);

            var adminUserRole = new UserRole { Id = 1, Name = WellknownUserRole.Admin };
            var testUserRole = new UserRole { Id = 4, Name = "Test Role" };

            var odataObjectCollection = new[] { adminUserRole, testUserRole }.AsOdata<UserRole, int>();

            var adminRelatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            var entity = new Entity { Id = 0, Name = "All" };
            var odataEntity = entity.AsOdata<Entity, int>();
            adminRelatedEntityCollection.Add(odataEntity);
            odataObjectCollection[0].RelatedEntityCollection.Add(adminRelatedEntityCollection);

            var testRelatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            var entity1 = new Entity { Id = 4, Name = "Product" };
            var odataEntity1 = entity1.AsOdata<Entity, int>();
            testRelatedEntityCollection.Add(odataEntity1);
            var entity2 = new Entity { Id = 32, Name = "ProductGroup" };
            var odataEntity2 = entity2.AsOdata<Entity, int>();
            testRelatedEntityCollection.Add(odataEntity2);
            odataObjectCollection[1].RelatedEntityCollection.Add(testRelatedEntityCollection);

            var json = JsonConvert.SerializeObject(odataObjectCollection);
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCacheFactory.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.CreateAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            var adminRoleEntityData = result[1];
            Assert.AreEqual(1, adminRoleEntityData.Count);
            var adminPermission = adminRoleEntityData["All"];
            Assert.AreEqual("All", adminPermission.Entity);
            Assert.AreEqual(Permissions.Admin, adminPermission.Permissions.First());

            var testRoleEntityData = result[4];
            Assert.AreEqual(2, testRoleEntityData.Count);
            var productEntityPermission = testRoleEntityData["Product"];
            Assert.AreEqual("Product", productEntityPermission.Entity);
            Assert.AreEqual(Permissions.Admin, productEntityPermission.Permissions.First());

            var productGroupEntityPermission = testRoleEntityData["ProductGroup"];
            Assert.AreEqual("ProductGroup", productGroupEntityPermission.Entity);
            Assert.AreEqual(Permissions.Admin, productGroupEntityPermission.Permissions.First());

            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task UserRoleEntityDataCacheFactory_UpdateRoleEntityDataAsync_AddAdditionalRoleEntity_Test()
        {
            // Arrange
            var factory = CreateFactory();
            _MockNamedFactory.Setup(m => m.Create(nameof(UserRole)))
                             .Returns(_MockUserRoleClient.Object);

            var adminUserRole = new UserRole { Id = 1, Name = WellknownUserRole.Admin };
            var testUserRole = new UserRole { Id = 4, Name = "Test Role" };

            var odataObjectCollection = new[] { adminUserRole, testUserRole }.AsOdata<UserRole, int>();

            var adminRelatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            var entity = new Entity { Id = 0, Name = "All" };
            var odataEntity = entity.AsOdata<Entity, int>();
            adminRelatedEntityCollection.Add(odataEntity);
            odataObjectCollection[0].RelatedEntityCollection.Add(adminRelatedEntityCollection);

            var testRelatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            var entity1 = new Entity { Id = 4, Name = "Product" };
            var odataEntity1 = entity1.AsOdata<Entity, int>();
            testRelatedEntityCollection.Add(odataEntity1);
            var entity2 = new Entity { Id = 32, Name = "ProductGroup" };
            var odataEntity2 = entity2.AsOdata<Entity, int>();
            testRelatedEntityCollection.Add(odataEntity2);
            odataObjectCollection[1].RelatedEntityCollection.Add(testRelatedEntityCollection);
            var json1 = JsonConvert.SerializeObject(odataObjectCollection);
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCacheFactory.Expand, true))
                               .ReturnsAsync(json1);

            var entity3 = new Entity { Id = 34, Name = "ProductGroupMembership" };
            var odataEntity3 = entity3.AsOdata<Entity, int>();
            testRelatedEntityCollection.Add(odataEntity3);
            var json2 = JsonConvert.SerializeObject(odataObjectCollection[1]);
            _MockUserRoleClient.Setup(m => m.GetAsync(testUserRole.Id.ToString(), UserRoleEntityDataCacheFactory.Expand, true))
                   .ReturnsAsync(json2);

            factory.Cache = await (factory as UserRoleEntityDataCacheFactory).CreateAsync();

            // Act
            await factory.UpdateRoleEntityDataAsync(testUserRole.Id);

            // Assert
            Assert.AreEqual(2, factory.Cache.Count);
            var adminRoleEntityData = factory.Cache[1];
            Assert.AreEqual(1, adminRoleEntityData.Count);
            var adminPermission = adminRoleEntityData["All"];
            Assert.AreEqual("All", adminPermission.Entity);
            Assert.AreEqual(Permissions.Admin, adminPermission.Permissions.First());

            var testRoleEntityData = factory.Cache[4];
            Assert.AreEqual(3, testRoleEntityData.Count);
            var productEntityPermission = testRoleEntityData["Product"];
            Assert.AreEqual("Product", productEntityPermission.Entity);
            Assert.AreEqual(Permissions.Admin, productEntityPermission.Permissions.First());

            var productGroupEntityPermission = testRoleEntityData["ProductGroup"];
            Assert.AreEqual("ProductGroup", productGroupEntityPermission.Entity);
            Assert.AreEqual(Permissions.Admin, productGroupEntityPermission.Permissions.First());

            var productGroupMemebershipEntityPermission = testRoleEntityData["ProductGroupMembership"];
            Assert.AreEqual("ProductGroupMembership", productGroupMemebershipEntityPermission.Entity);
            Assert.AreEqual(Permissions.Admin, productGroupMemebershipEntityPermission.Permissions.First());

            _MockRepository.VerifyAll();
        }
        #endregion

        #region Cache
        [TestMethod]
        public void UserRoleEntityDataCacheFactory_Cache_Calls_CreateAsyncMethod_Test()
        {
            // Arrange
            var factory = CreateFactory() as UserRoleEntityDataCacheFactory;
            IUserRoleEntityDataCache cache = new UserRoleEntityDataCache();
            Func<Task<IUserRoleEntityDataCache>> method = factory.CreateAsync;
            _MockPreventSimultaneousFuncCalls.Setup(m => m.Call(method)).Returns(Task.FromResult(cache));

            // Act
            var actual = factory.Cache;

            // Assert
            Assert.AreEqual(cache, actual);
        }
        #endregion
    }
}
