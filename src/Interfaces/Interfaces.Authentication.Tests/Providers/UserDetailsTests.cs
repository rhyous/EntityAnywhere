using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.StringLibrary;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Exceptions;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Providers
{
    [TestClass]
    public class UserDetailsTests
    {
        private MockRepository _MockRepository;

        private Mock<IClaimsProvider> _MockClaimsProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockClaimsProvider = _MockRepository.Create<IClaimsProvider>();
        }

        private UserDetails CreateUserDetails()
        {
            return new UserDetails(_MockClaimsProvider.Object);
        }

        #region Organization Id
        [TestMethod]
        public void UserDetails_OrganizationId_ClaimsProviderIsCalled()
        {
            // Arrange
            var provider = CreateUserDetails();
            var orgId = 1027;
            _MockClaimsProvider.Setup(m => m.GetClaim<int>("Organization", "Id")).Returns(orgId);

            // Act
            var actual1 = provider.OrganizationId;
            var actual2 = provider.OrganizationId;

            // Assert
            Assert.AreEqual(orgId, actual1);
            Assert.AreEqual(orgId, actual2);
            _MockRepository.VerifyAll();
            _MockClaimsProvider.Verify(m => m.GetClaim<int>("Organization", "Id"), Times.Once);
        }
        #endregion

        #region Username
        [TestMethod]
        public void UserDetails_Username_ClaimsProviderIsCalled()
        {
            // Arrange
            var provider = CreateUserDetails();
            var username = "testuser1";
            _MockClaimsProvider.Setup(m => m.GetClaim("User", "Username")).Returns(username);

            // Act
            var actual1 = provider.Username;
            var actual2 = provider.Username;

            // Assert
            Assert.AreEqual(username, actual1);
            Assert.AreEqual(username, actual2);
            _MockRepository.VerifyAll();
            _MockClaimsProvider.Verify(m => m.GetClaim("User", "Username"), Times.Once);
        }
        #endregion

        #region User Id
        [TestMethod]
        public void UserDetails_UserId_ClaimsProviderIsCalled()
        {
            // Arrange
            var provider = CreateUserDetails();
            long userId = 1099;
            _MockClaimsProvider.Setup(m => m.GetClaim<long>("User", "Id")).Returns(userId);

            // Act
            var actual1 = provider.UserId;
            var actual2 = provider.UserId;

            // Assert
            Assert.AreEqual(userId, actual1);
            Assert.AreEqual(userId, actual2);
            _MockRepository.VerifyAll();
            _MockClaimsProvider.Verify(m => m.GetClaim<long>("User", "Id"), Times.Once);
        }
        #endregion

        #region UserRole
        [TestMethod]
        public void UserDetails_UserRole_ClaimsProviderIsCalled()
        {
            // Arrange
            var provider = CreateUserDetails();
            var userRole = "testUserRole10";
            _MockClaimsProvider.Setup(m => m.GetClaim("UserRole", "Role")).Returns(userRole);

            // Act
            var actual1 = provider.UserRole;
            var actual2 = provider.UserRole;

            // Assert
            Assert.AreEqual(userRole, actual1);
            Assert.AreEqual(userRole, actual2);
            _MockRepository.VerifyAll();
            _MockClaimsProvider.Verify(m => m.GetClaim("UserRole", "Role"), Times.Once);
        }
        #endregion
    }
}
