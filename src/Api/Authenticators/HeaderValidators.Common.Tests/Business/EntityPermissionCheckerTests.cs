using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class EntityPermissionCheckerTests
    {
        private MockRepository _MockRepository;

        private Mock<IUserRoleEntityDataCache> _MockUserRoleEntityDataCache;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
        }

        private EntityPermissionChecker CreateEntityPermissionChecker()
        {
            return new EntityPermissionChecker(_MockUserRoleEntityDataCache.Object);
        }

        #region HasPermssion(string role, string entity)
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void EntityPermissionChecker_HasPermssion_Role_String_NullEmptyOrWhitespace_ReturnsFalse(string role)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            string entity = "Entity1";

            // Act
            var result = entityPermissionChecker.HasPermission(role, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void EntityPermissionChecker_HasPermssion_Role_String_Entity_NullEmptyOrWhitespace_ReturnsFalse(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            string role = "Customer";

            // Act
            var result = entityPermissionChecker.HasPermission(role, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityPermissionChecker_HasPermssion_Role_String_EntityDataCache_NotFound_ReturnsFalse()
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            string role = "My Awesome Role 21";
            int roleId = 21;
            string entity = "CoolioEntity";

            var roleIdDictionary = new ConcurrentDictionary<string, int>();
            roleIdDictionary.TryAdd("My Awesome Role 21", roleId);
            _MockUserRoleEntityDataCache.Setup(m => m.UserRoleIds)
                                        .Returns(roleIdDictionary);

            IUserRoleEntityData outValue = null;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(roleId, out outValue))
                                        .Returns(false);

            // Act
            var result = entityPermissionChecker.HasPermission(role, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Entity1", "Entity2", "Entity3", "Entity4")]
        public void EntityPermissionChecker_HasPermssion_Role_String_AdminRole_Returns_TrueForAnyEntity(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            string role = WellknownUserRole.Admin;

            var roleIdDictionary = new ConcurrentDictionary<string, int>();
            roleIdDictionary.TryAdd(WellknownUserRole.Admin, WellknownUserRoleIds.Admin);
            _MockUserRoleEntityDataCache.Setup(m => m.UserRoleIds)
                                        .Returns(roleIdDictionary);

            // Act
            var result = entityPermissionChecker.HasPermission(role, entity);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }
        #endregion


        #region HasPermssion(string role, string entity)
        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void EntityPermissionChecker_HasPermssion_Role_Int_NotPositiveInt_ReturnsFalse(int roleId)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            string entity = "Entity1";

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void EntityPermissionChecker_HasPermssion_Role_Int_Entity_NullEmptyOrWhitespace_ReturnsFalse(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = 10;

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityPermissionChecker_HasPermssion_Role_Int_Admin_ReturnsTrue()
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = WellknownUserRoleIds.Admin;
            string entity = "CoolioEntity";

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityPermissionChecker_HasPermssion_Role_Int_EntityDataCache_NotFound_ReturnsFalse()
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = 21;
            string entity = "CoolioEntity";

            IUserRoleEntityData outValue = null;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(roleId, out outValue))
                                        .Returns(false);

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Entity1", "Entity2", "Entity3", "Entity4")]
        public void EntityPermissionChecker_HasPermssion_Role_Int_AdminPermission_Returns_TrueForAnyEntity(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            string role = WellknownUserRole.Admin;

            var roleIdDictionary = new ConcurrentDictionary<string, int>();
            roleIdDictionary.TryAdd(WellknownUserRole.Admin,  WellknownUserRoleIds.Admin);
            _MockUserRoleEntityDataCache.Setup(m => m.UserRoleIds)
                                        .Returns(roleIdDictionary);

            // Act
            var result = entityPermissionChecker.HasPermission(role, entity);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityPermissionChecker_HasPermssion_Role_Int_UserRoleEntityDataCache_OutsNull_Returns_False()
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = 27;
            var entity = "CoolioEntity";

            IUserRoleEntityData outValue = null;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(roleId, out outValue))
                                        .Returns(true);

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Entity1", "Entity2", "Entity3", "Entity4")]
        public void EntityPermissionChecker_HasPermssion_Role_Int_PermissionNull_Returns_FalseForAnyEntity(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = 27;

            var mockUserRoleEntityData = _MockRepository.Create<IUserRoleEntityData>();
            IUserRoleEntityData outValue = mockUserRoleEntityData.Object;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(roleId, out outValue))
                                        .Returns(true);
            EntityPermission permission = null;
            mockUserRoleEntityData.Setup(m => m.TryGetValue(entity, out permission))
                                  .Returns(null);

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Entity1", "Entity2", "Entity3", "Entity4")]
        public void EntityPermissionChecker_HasPermssion_Role_Int_PermissionOtherThanAdmin_Returns_FalseForAnyEntity(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = 27;

            var mockUserRoleEntityData = _MockRepository.Create<IUserRoleEntityData>();
            IUserRoleEntityData outValue = mockUserRoleEntityData.Object;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(roleId, out outValue))
                                        .Returns(true);
            EntityPermission permission = new EntityPermission { Entity = entity, Permissions = new HashSet<string> { "ReadOnly" } };
            mockUserRoleEntityData.Setup(m => m.TryGetValue(entity, out permission))
                                  .Returns(true);

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList("Entity1", "Entity2", "Entity3", "Entity4")]
        public void EntityPermissionChecker_HasPermssion_Role_Int_PermissionMissing_Returns_FalseForAnyEntity(string entity)
        {
            // Arrange
            var entityPermissionChecker = CreateEntityPermissionChecker();
            int roleId = 27;

            var mockUserRoleEntityData = _MockRepository.Create<IUserRoleEntityData>();
            IUserRoleEntityData outValue = mockUserRoleEntityData.Object;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(roleId, out outValue))
                                        .Returns(true);
            EntityPermission permission = new EntityPermission { Entity = entity, Permissions = new HashSet<string> { "ReadOnly" } };
            mockUserRoleEntityData.Setup(m => m.TryGetValue(entity, out permission))
                                  .Returns(true);

            // Act
            var result = entityPermissionChecker.HasPermission(roleId, entity);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}