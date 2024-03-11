using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.Odata;
using Rhyous.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Models
{
    [TestClass]
    public class UserRoleEntityDataCacheTests
    {
        private MockRepository _MockRepository;

        private Mock<INamedFactory<IAdminEntityClientAsync>> _MockNamedFactory;
        private Mock<IAdminEntityClientAsync> _MockUserRoleClient;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminEntityClientAsync>>();
            _MockUserRoleClient = _MockRepository.Create<IAdminEntityClientAsync>();
        }
        private UserRoleEntityDataCache CreateUserRoleEntityDataCache()
        {
            return new UserRoleEntityDataCache(_MockNamedFactory.Object);
        }

        #region CreateAsync
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UserRoleEntityDataCache_CreateAsync_UserRoleClient_ReturnsNullEmptyOrWhitespace_CacheIsEmpty(string json)
        {
            // Arrange
            var factory = CreateUserRoleEntityDataCache();
            _MockNamedFactory.Setup(m => m.Create("UserRole"))
                             .Returns(_MockUserRoleClient.Object);
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCache.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.ProvideAsync();


            // Assert
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCache_CreateAsync_UserRoleClient_ReturnsHtmlError_CacheIsEmpty()
        {
            // Arrange
            var factory = CreateUserRoleEntityDataCache();
            _MockNamedFactory.Setup(m => m.Create("UserRole"))
                             .Returns(_MockUserRoleClient.Object);
            var notJson = "<html><body><p>Some html error</p></body></html>";
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCache.Expand, true))
                               .ReturnsAsync(notJson);

            // Act
            var result = await factory.ProvideAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCache_CreateAsync_Admin_All()
        {
            // Arrange
            var factory = CreateUserRoleEntityDataCache();
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
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCache.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.ProvideAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            var userRoleEntityData = result[1];
            Assert.AreEqual(1, userRoleEntityData.Count);
            Assert.AreEqual("All", userRoleEntityData["All"].Entity);
            Assert.AreEqual(Permissions.Admin, userRoleEntityData["All"].Permissions.First());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleEntityDataCache_CreateAsync_TestUserRole_HasNoEntityAssignedYet_Test()
        {
            // Arrange
            var factory = CreateUserRoleEntityDataCache();
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
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCache.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.ProvideAsync();

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
        public async Task UserRoleEntityDataCache_CreateAsync_Admin_SpecifiedEntities_Test()
        {
            // Arrange
            var factory = CreateUserRoleEntityDataCache();
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
            _MockUserRoleClient.Setup(m => m.GetByQueryParametersAsync(UserRoleEntityDataCache.Expand, true))
                               .ReturnsAsync(json);

            // Act
            var result = await factory.ProvideAsync();

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

        #region CreateRoleEntityDataCache
        [TestMethod]
        public void UserRoleEntityDataCacheCreator_CreateRoleEntityDataCache_NoEntityPermissions_Test()
        {
            // Arrange
            var userRoleEntityDataCacheCreator = CreateUserRoleEntityDataCache();
            var testUserRole = new UserRole { Id = 4, Name = "Test Role" };
            var jsonUserRole = JsonConvert.SerializeObject(testUserRole);
            var odataUserRole = new OdataObject { Object = new JRaw(jsonUserRole) };


            // Act
            var result = userRoleEntityDataCacheCreator.CreateRoleEntityDataCache(odataUserRole);

            // Assert
            Assert.AreEqual(testUserRole.Id, result.UserRoleId);
            Assert.AreEqual(testUserRole.Name, result.UserRoleName);
            Assert.AreEqual(0, result.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UserRoleEntityDataCacheCreator_CreateRoleEntityDataCache_OneEntityPermissions_Test()
        {
            // Arrange
            var userRoleEntityDataCacheCreator = CreateUserRoleEntityDataCache();
            var testUserRole = new UserRole { Id = 4, Name = "Test Role" };
            var jsonUserRole = JsonConvert.SerializeObject(testUserRole);
            var odataUserRole = new OdataObject { Object = new JRaw(jsonUserRole) };

            var testRelatedEntityCollection = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(Entity) };
            var entity1 = new Entity { Id = 4, Name = "Product" };
            var odataEntity1 = entity1.AsOdata<Entity, int>();
            testRelatedEntityCollection.Add(odataEntity1);
            odataUserRole.RelatedEntityCollection.Add(testRelatedEntityCollection);

            // Act
            var result = userRoleEntityDataCacheCreator.CreateRoleEntityDataCache(odataUserRole);

            // Assert
            Assert.AreEqual(testUserRole.Id, result.UserRoleId);
            Assert.AreEqual(testUserRole.Name, result.UserRoleName);
            Assert.AreEqual(1, result.Count);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region TryGetValue
        [TestMethod]
        public void UserRoleEntityDataCache_TryGetValue_RoleDoesNotExist()
        {
            // Arrange
            var userRoleEntityDataCache = CreateUserRoleEntityDataCache();
            string userRoleName = "Role27";
            IUserRoleEntityData value = null;

            // Act
            var result = userRoleEntityDataCache.TryGetValue(userRoleName, out value);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void UserRoleEntityDataCache_TryGetValue_RoleExists()
        {
            // Arrange
            var userRoleEntityDataCache = CreateUserRoleEntityDataCache();
            string userRoleName = "Role27";
            IUserRoleEntityData data = new UserRoleEntityData { UserRoleId = 27, UserRoleName = userRoleName };
            userRoleEntityDataCache.UserRoleIds.TryAdd(data.UserRoleName, data.UserRoleId);
            userRoleEntityDataCache.Cache.GetOrAdd(data.UserRoleId, data);
            IUserRoleEntityData value = null;

            // Act
            var result = userRoleEntityDataCache.TryGetValue(userRoleName, out value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(data, value);
        }
        #endregion

        #region Custom Indexer
        [TestMethod]
        public void UserRoleEntityDataCache_Indexer_RoleExists()
        {
            // Arrange
            var userRoleEntityDataCache = CreateUserRoleEntityDataCache();
            string userRoleName = "Role27";
            IUserRoleEntityData data = new UserRoleEntityData { UserRoleId = 27, UserRoleName = userRoleName };
            userRoleEntityDataCache.UserRoleIds.TryAdd(data.UserRoleName, data.UserRoleId);
            userRoleEntityDataCache.Cache.GetOrAdd(data.UserRoleId, data);

            // Act
            var result = userRoleEntityDataCache[userRoleName];

            // Assert
            Assert.AreEqual(data, result);
        }
        #endregion


        #region Update
        [TestMethod]
        public async Task UserRoleEntityDataCacheFactory_UpdateRoleEntityDataAsync_AddAdditionalRoleEntity_Test()
        {
            // Arrange
            var userRoleEntityDataCache = CreateUserRoleEntityDataCache();
            var roleId = 4;
            var roleName = "Role 4";
            var userRole = new UserRole { Id = 4, Name = roleName };
            var odataUserRole = userRole.AsOdata<UserRole, int>();
            var entity = new Entity { Id = 10, Name = "Entity 10" };
            var odataEntities = new[] { entity }.AsOdata<Entity, int>();
            odataUserRole.RelatedEntityCollection.Add(odataEntities);
            var json = JsonConvert.SerializeObject(odataUserRole);
            _MockNamedFactory.Setup(m => m.Create(nameof(UserRole)))
                             .Returns(_MockUserRoleClient.Object);
            _MockUserRoleClient.Setup(m => m.GetAsync(roleId.ToString(), UserRoleEntityDataCache.Expand, true))
                               .ReturnsAsync(json);
            var testUserRole = new UserRole { Id = roleId, Name = roleName };

            // Act
            await userRoleEntityDataCache.UpdateRoleEntityDataAsync(testUserRole.Id);

            // Assert
            Assert.AreEqual(1, userRoleEntityDataCache.Cache.Count);
            Assert.AreEqual(1, userRoleEntityDataCache.Cache[roleId].Count);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}