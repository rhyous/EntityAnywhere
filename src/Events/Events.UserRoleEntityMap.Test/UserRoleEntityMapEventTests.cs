using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Events.Tests
{
    [TestClass]
    public class UserRoleEntityMapEventTests
    {
        private MockRepository _MockRepository;

        private Mock<IUserRoleEntityDataCacheFactory> _MockUserRoleEntityDataCacheFactory;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockUserRoleEntityDataCacheFactory = _MockRepository.Create<IUserRoleEntityDataCacheFactory>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private UserRoleEntityMapEvent CreateUserRoleEntityMapEvent()
        {
            return new UserRoleEntityMapEvent(
                _MockUserRoleEntityDataCacheFactory.Object,
                _MockLogger.Object);
        }

        #region AfterDelete
        [TestMethod]
        public void UserRoleEntityMapEvent_AfterDelete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();
            var entity = new UserRoleEntityMap
            {
                Id = 1,
                UserRoleId = 1,
                EntityId = 1
            };
            bool wasDeleted = true;  
          
            var permission = new EntityPermission
            {
                Entity = "UserRole",
                Permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { Permissions.Admin } 
            };

            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = 1,
                UserRoleName = "TestRole"
            };

            userRoleEntityData.GetOrAdd(permission.Entity, permission);
            userRoleEntityData.EntityIds.Add((int)entity.Id, permission.Entity);

            var userRoleEntityDataCache = new UserRoleEntityDataCache();
            userRoleEntityDataCache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.Cache)
                                               .Returns(userRoleEntityDataCache);

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                                           "ClearUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            // Act
            userRoleEntityMapEvent.AfterDelete(entity, wasDeleted);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterDeleteMany
        [TestMethod]
        public void UserRoleEntityMapEvent_AfterDeleteMany_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();
            var entity = new UserRoleEntityMap
            {
                Id = 1,
                UserRoleId = 1,
                EntityId = 1
            };

            var entity2 = new UserRoleEntityMap
            {
                Id = 2,
                UserRoleId = 1,
                EntityId = 2
            };

            var entities = new List<UserRoleEntityMap>();
            entities.Add(entity);
            entities.Add(entity2);

            var permission = new EntityPermission
            {
                Entity = "UserRole",
                Permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { Permissions.Admin }
            };

            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = 1,
                UserRoleName = "TestRole"
            };

            userRoleEntityData.GetOrAdd(permission.Entity, permission);
            userRoleEntityData.EntityIds.Add((int)entity.Id, permission.Entity);

            var userRoleEntityDataCache = new UserRoleEntityDataCache();
            userRoleEntityDataCache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.Cache)
                                               .Returns(userRoleEntityDataCache);

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                                           "ClearUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            // Act
            userRoleEntityMapEvent.AfterDeleteMany(entities, new Dictionary<long, bool>());

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterPost
        [TestMethod]
        public void UserRoleEntityMapEvent_AfterPost_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();
            var entity = new UserRoleEntityMap
            {
                Id = 1,
                UserRoleId = 1,
                EntityId = 1
            };

            var entity2 = new UserRoleEntityMap
            {
                Id = 2,
                UserRoleId = 1,
                EntityId = 2
            };

            var postedItems = new List<UserRoleEntityMap>();
            postedItems.Add(entity);
            postedItems.Add(entity2);

            //bool wasDeleted = true;

            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = 1,
                UserRoleName = "TestRole"
            };

            var userRoleEntityDataCache = new UserRoleEntityDataCache();
            userRoleEntityDataCache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.UpdateRoleEntityDataAsync(1))
                                               .Returns(System.Threading.Tasks.Task.CompletedTask);
                                               

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                                           "UpdateUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            // Act
            userRoleEntityMapEvent.AfterPost(postedItems);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterPatch

        [TestMethod]
        public void UserRoleEntityMapEvent_AfterPatch_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();
         
            var patchedEntityComparison = new PatchedEntityComparison<UserRoleEntityMap, long>()
            {
                Entity = new UserRoleEntityMap
                {
                    Id = 1,
                    UserRoleId = 1,
                    EntityId = 1
                },
                PatchedEntity = new PatchedEntity<UserRoleEntityMap, long>()
                {
                    Entity = new UserRoleEntityMap
                    {
                        Id = 1,
                        UserRoleId = 1,
                        EntityId = 2
                    },
                    ChangedProperties = new HashSet<string> { "EntityId" }
                }
            };

            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = 1,
                UserRoleName = "TestRole"
            };

            var userRoleEntityDataCache = new UserRoleEntityDataCache();
            userRoleEntityDataCache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.UpdateRoleEntityDataAsync(1))
                                            .Returns(System.Threading.Tasks.Task.CompletedTask);

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                                           "UpdateUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            // Act
            userRoleEntityMapEvent.AfterPatch(patchedEntityComparison);

            // Assert
            _MockRepository.VerifyAll();
        }

        #endregion

        #region AfterPatchMany
        [TestMethod]
        public void UserRoleEntityMapEvent_AfterPatchMany_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();           

            var patchedEntityComparison1 = new PatchedEntityComparison<UserRoleEntityMap, long>()
            {
                Entity = new UserRoleEntityMap
                {
                    Id = 1,
                    UserRoleId = 1,
                    EntityId = 1
                },
                PatchedEntity = new PatchedEntity<UserRoleEntityMap, long>()
                {
                    Entity = new UserRoleEntityMap
                    {
                        Id = 1,
                        UserRoleId = 1,
                        EntityId = 2
                    },
                    ChangedProperties = new HashSet<string> { "EntityId" }
                }
            };

            var patchedEntityComparison2 = new PatchedEntityComparison<UserRoleEntityMap, long>()
            {
                Entity = new UserRoleEntityMap
                {
                    Id = 1,
                    UserRoleId = 1,
                    EntityId = 3
                },
                PatchedEntity = new PatchedEntity<UserRoleEntityMap, long>()
                {
                    Entity = new UserRoleEntityMap
                    {
                        Id = 1,
                        UserRoleId = 1,
                        EntityId = 3
                    },
                    ChangedProperties = new HashSet<string> { "EntityId" }
                }
            };

            var patchedEntityComparisons = new List<PatchedEntityComparison<UserRoleEntityMap, long>>() { patchedEntityComparison1, patchedEntityComparison2 };

            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = 1,
                UserRoleName = "TestRole"
            };

            var userRoleEntityDataCache = new UserRoleEntityDataCache();
            userRoleEntityDataCache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.UpdateRoleEntityDataAsync(1))
                                            .Returns(System.Threading.Tasks.Task.CompletedTask);

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                                           "UpdateUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            // Act
            userRoleEntityMapEvent.AfterPatchMany(patchedEntityComparisons);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region ClearUserRoleEntityData
        [TestMethod]
        public void UserRoleEntityMapEvent_ClearUserRoleEntityData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();
            int key = 1;
            int entityId = 1;

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                               "ClearUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = 1,
                UserRoleName = "TestRole"
            };

            var userRoleEntityDataCache = new UserRoleEntityDataCache();
            userRoleEntityDataCache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);
            userRoleEntityData.EntityIds.Add(userRoleEntityData.UserRoleId, entityId.ToString());

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.Cache)
                                               .Returns(userRoleEntityDataCache);

            // Act
            userRoleEntityMapEvent.ClearUserRoleEntityData(userRoleEntityData.UserRoleId, entityId);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region UpdateUserRoleEntityData
        [TestMethod]
        public void UserRoleEntityMapEvent_UpdateUserRoleEntityData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var userRoleEntityMapEvent = CreateUserRoleEntityMapEvent();
            int key = 1;

            _MockLogger.Setup(x => x.Debug("UserRole Entity Map configuration data changed. Updating UserRoleEntityData.",
                                           "UpdateUserRoleEntityData", It.IsAny<string>(), It.IsAny<int>()));

            _MockUserRoleEntityDataCacheFactory.Setup(x => x.UpdateRoleEntityDataAsync(1))
                                               .Returns(System.Threading.Tasks.Task.CompletedTask);

            // Act
            userRoleEntityMapEvent.UpdateUserRoleEntityData(key);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
