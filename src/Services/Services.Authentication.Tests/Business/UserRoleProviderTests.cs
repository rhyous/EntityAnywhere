using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class UserRoleProviderTests
    {
        private MockRepository _MockRepository;

        private Mock<IUserRoleEntityDataCache> _MockUserRoleEntityDataCache;
        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private UserRoleProvider CreateProvider()
        {
            return new UserRoleProvider(
                _MockUserRoleEntityDataCache.Object,
                _MockAppSettings.Object);
        }

        #region GetDefaultRoleIdAsync
        [TestMethod]
        public void UserRoleProvider_GetDefaultRoleIdAsync_DefaultNoAppSetting_Test()
        {
            // Arrange
            var nvc = new NameValueCollection(); // Empty
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            var userRolesIds = new Dictionary<string, int> { { "Customer", 2 } };
            _MockUserRoleEntityDataCache.Setup(m => m.UserRoleIds).Returns(userRolesIds);

            var provider = CreateProvider();

            // Act
            var actual = provider.GetDefaultRoleId();

            // Assert
            Assert.AreEqual(2, actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserRoleProvider_GetDefaultRoleIdAsync_AppSettingChanged_Test()
        {
            // Arrange
            const string anonymous = "Anonymous";
            var nvc = new NameValueCollection { { UserRoleProvider.DefaultUserRoleSetting, anonymous } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            var userRolesIds = new Dictionary<string, int> { { "Customer", 2 }, { "Anonymous", 5 } };
            _MockUserRoleEntityDataCache.Setup(m => m.UserRoleIds).Returns(userRolesIds);

            var provider = CreateProvider();

            // Act
            var actual = provider.GetDefaultRoleId();

            // Assert
            Assert.AreEqual(5, actual);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
