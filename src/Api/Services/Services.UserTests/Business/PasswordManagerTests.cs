using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class PasswordManagerTests
    {
        const int Sha256HashLength = 64;

        private MockRepository _MockRepository;
        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private PasswordManager CreateManager()
        {
            return new PasswordManager(_MockAppSettings.Object);
        }

        #region IsHashed = false
        [TestMethod]
        public void PasswordManager_SetOrHashPassword_NullUser_ReturnsNull()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = null;
            bool passwordChanged = false;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNull(user);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_NullPassword_SetsPassword()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1" };
            bool passwordChanged = false;

            var nvc = new NameValueCollection { { "DefaultUserPasswordLength", "10" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNotNull(user.Password);
            Assert.AreEqual(manager.DefaultPasswordLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_NullPasswordExternalAuth_DoesNotSetPassword()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", ExternalAuth = true };
            bool passwordChanged = false;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNull(user.Password);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_NullPasswordExternalAuthPasswordChangedTrue_AllowsPasswordToBeSetToNull()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", ExternalAuth = true };
            bool passwordChanged = true;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNull(user.Password);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region IsHashed = true
        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTrue_HashesPassword()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", IsHashed = true, Password = "newpw" };
            bool passwordChanged = true;
            var nvc = new NameValueCollection { { "DefaultUserPasswordLength", "10" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNotNull(user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedFalse_SetsPassword()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", IsHashed = true };
            bool passwordChanged = false;

            var nvc = new NameValueCollection { { "DefaultUserPasswordLength", "10" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNotNull(user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedFalse_LeavesPasswordAsIs()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", IsHashed = true, Password = "alreadyhashedpassword" };
            bool passwordChanged = false;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.AreEqual("alreadyhashedpassword", user.Password);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedTrueNoExistingSalt_LeavesPasswordAsIs()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", IsHashed = true, Password = "newpassword" };
            bool passwordChanged = true;

            var nvc = new NameValueCollection { { "DefaultUserPasswordLength", "10" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNotNull(user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedTrueHasExistingSalt_LeavesPasswordAsIs()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User { Username = "user1", IsHashed = true, Password = "newpassword", Salt = "abc" };
            bool passwordChanged = true;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.AreEqual("440717e5c401e8f335921c7d56a371fc27d0f2807492aaebb23d1810cae54fca", user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }




















        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTrueExternalAuthTrue_HashesPassword()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User
            {
                Username = "user1",
                IsHashed = true,
                Password = "newpw",
                ExternalAuth = true
            };
            bool passwordChanged = true;

            var nvc = new NameValueCollection { { "DefaultUserPasswordLength", "10" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNotNull(user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedFalseExternalAuthTrue_PasswordStaysNull()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User
            {
                Username = "user1",
                IsHashed = true,
                ExternalAuth = true
            };
            bool passwordChanged = false;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNull(user.Password);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedFalseExternalAuthTrue_LeavesPasswordAsIs()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User
            {
                Username = "user1",
                IsHashed = true,
                Password = "alreadyhashedpassword",
                ExternalAuth = true
            };
            bool passwordChanged = false;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.AreEqual("alreadyhashedpassword", user.Password);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedTrueNoExistingSaltExternalAuthTrue_LeavesPasswordAsIs()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User
            {
                Username = "user1",
                IsHashed = true,
                Password = "newpassword",
                ExternalAuth = true
            };
            bool passwordChanged = true;

            var nvc = new NameValueCollection { { "DefaultUserPasswordLength", "10" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.IsNotNull(user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void PasswordManager_SetOrHashPassword_EditedPasswordIsHashedTruePasswordChangedTrueHasExistingSaltExternalAuthTrue_LeavesPasswordAsIs()
        {
            // Arrange
            var manager = CreateManager();
            IUser user = new User
            {
                Username = "user1",
                IsHashed = true,
                Password = "newpassword",
                Salt = "abc",
                ExternalAuth = true
            };
            bool passwordChanged = true;

            // Act
            manager.SetOrHashPassword(user, passwordChanged);

            // Assert
            Assert.AreEqual("440717e5c401e8f335921c7d56a371fc27d0f2807492aaebb23d1810cae54fca", user.Password);
            Assert.AreEqual(Sha256HashLength, user.Password.Length);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region List<IUser>
        [TestMethod]
        public void PasswordManager_SetOrHashPassword_NullUserEnumerable_ReturnsNull()
        {
            // Arrange
            var manager = CreateManager();
            IEnumerable<IUser> users = null;

            // Act
            manager.SetOrHashPassword(users);

            // Assert
            Assert.IsNull(users);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}