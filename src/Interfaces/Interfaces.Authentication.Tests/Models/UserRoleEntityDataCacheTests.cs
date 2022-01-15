using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Models
{
    [TestClass]
    public class UserRoleEntityDataCacheTests
    {

        private UserRoleEntityDataCache CreateUserRoleEntityDataCache()
        {
            return new UserRoleEntityDataCache();
        }

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
            var userRoleIds = new Dictionary<string, IUserRoleEntityData>();
            string userRoleName = "Role27";
            IUserRoleEntityData data = new UserRoleEntityData { UserRoleId = 27, UserRoleName = userRoleName };
            userRoleEntityDataCache.UserRoleIds.Add(data.UserRoleName, data.UserRoleId);
            userRoleEntityDataCache.GetOrAdd(data.UserRoleId, data);
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
            var userRoleIds = new Dictionary<string, IUserRoleEntityData>();
            string userRoleName = "Role27";
            IUserRoleEntityData data = new UserRoleEntityData { UserRoleId = 27, UserRoleName = userRoleName };
            userRoleEntityDataCache.UserRoleIds.Add(data.UserRoleName, data.UserRoleId);
            userRoleEntityDataCache.GetOrAdd(data.UserRoleId, data);

            // Act
            var result = userRoleEntityDataCache[userRoleName];

            // Assert
            Assert.AreEqual(data, result);
        }
        #endregion
    }
}