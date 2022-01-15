using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Tests
{
    [TestClass]
    public class AuthorizationWebServiceTests
    {
        private MockRepository _MockRepository;

        private Mock<IUserRoleEntityDataCache> _MockUserRoleEntityDataCache;
        private Mock<IUserDetails> _MockUserDetails;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
            _MockUserDetails = _MockRepository.Create<IUserDetails>();
        }

        private AuthorizationWebService CreateService()
        {
            return new AuthorizationWebService(
                _MockUserRoleEntityDataCache.Object,
                _MockUserDetails.Object);
        }

        #region MyRoleDataAsync
        [TestMethod]
        public async Task AuthorizationWebService_MyRoleDataAsync_NotFound_ReturnsNull()
        {
            // Arrange
            var service = CreateService();
            var userRole = "Role 27";
            _MockUserDetails.Setup(m => m.UserRole).Returns(userRole);
            IUserRoleEntityData userRoleEntityData = null;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(userRole, out userRoleEntityData))
                                        .Returns(false);

            // Act
            var result = await service.MyRoleDataAsync();

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthorizationWebService_MyRoleDataAsync_FoundButNull_ReturnsNull()
        {
            // Arrange
            var service = CreateService();
            var userRole = "Role 27";
            _MockUserDetails.Setup(m => m.UserRole).Returns(userRole);
            IUserRoleEntityData userRoleEntityData = null;
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(userRole, out userRoleEntityData))
                                        .Returns(true);

            // Act
            var result = await service.MyRoleDataAsync();

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthorizationWebService_MyRoleDataAsync_Found_ReturnsFound()
        {
            // Arrange
            var service = CreateService();
            var userRole = "Role 27";
            _MockUserDetails.Setup(m => m.UserRole).Returns(userRole);
            IUserRoleEntityData userRoleEntityData = new UserRoleEntityData();
            _MockUserRoleEntityDataCache.Setup(m => m.TryGetValue(userRole, out userRoleEntityData))
                                        .Returns(true);

            // Act
            var result = await service.MyRoleDataAsync();

            // Assert
            Assert.AreEqual(result, userRoleEntityData);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
