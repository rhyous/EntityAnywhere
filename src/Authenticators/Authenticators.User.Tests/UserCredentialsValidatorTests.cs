using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Authenticators;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Authenticators
{
    [TestClass]
    public class UserCredentialsValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<ITokenBuilder<IUser>> _MockTokenBuilder;
        private Mock<IAdminEntityClientAsync<User, long>> _MockUserClient;
        private Mock<IAppSettings> _MockAppSettings;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockTokenBuilder = _MockRepository.Create<ITokenBuilder<IUser>>();
            _MockUserClient = _MockRepository.Create<IAdminEntityClientAsync<User, long>>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
        }

        private UserCredentialsValidator CreateUserCredentialsValidator()
        {
            return new UserCredentialsValidator(
                _MockTokenBuilder.Object,
                _MockUserClient.Object,
                _MockAppSettings.Object);
        }

        #region IsValidAsync

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsNull_Throws()
        {
            // Arrange
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = null;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await userCredentialsValidator.IsValidAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UserCredentialsValidator_IsValidAsync_CredsDotUser_NullEmptyOrWhitespace_Throws(string user)
        {
            // Arrange
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = user, Password = "pw1234"};

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await userCredentialsValidator.IsValidAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UserCredentialsValidator_IsValidAsync_CredsDotPassword_NullEmptyOrWhitespace_Throws(string pw)
        {
            // Arrange
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = "user27", Password = pw };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await userCredentialsValidator.IsValidAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_ReturnsNull_Test()
        {
            // Arrange
            var username = "Jared";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = "pw-1234" };
            OdataObject<User, long> odataUser = null;
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true)).ReturnsAsync(odataUser);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_ReturnsDisabledUser_Test()
        {
            // Arrange
            var username = "Jared";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = "pw-1234" };
            var user = new User { Id = 101, Username = username, Enabled = false };
            var odataUser = user.AsOdata<User, long>();
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true)).ReturnsAsync(odataUser);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_ReturnsExternalOnlyUser_Test()
        {
            // Arrange
            var username = "Jared";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = "pw-1234" };
            var user = new User { Id = 101, Username = username, Enabled = true, ExternalAuth = true };
            var odataUser = user.AsOdata<User, long>();
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true)).ReturnsAsync(odataUser);
            var nvc = new NameValueCollection { { UserCredentialsValidator.AuthenticateExternallySetting, "true" } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvc);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            Assert.AreEqual($"This user {creds.User} can only authenticate with external authenticators.", result.Message);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_ReturnsNoUserRole_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = password };
            var user = new User { Id = 101, Username = username, Password = password, Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            OdataObject<UserRole, int> odataRole = null;
            var related = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(UserRole) };
            related.Add(odataRole);
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true)).ReturnsAsync(odataUser);


            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("The user has not been configured with a user role.", result.Message);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_Valid_NotHashed_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = password };
            var user = new User { Id = 101, Username = username, Password = password, Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            var role = new UserRole { Id = 1, Name = "TestRole", Enabled = true, Description = "This is a Test Role", LandingPageId = LandingPages.Admin };
            var odataRole = role.AsOdata<UserRole, int>();
            var related = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(UserRole) };
            related.Add(odataRole);
            odataUser.RelatedEntityCollection.Add(related);
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true))
                           .ReturnsAsync(odataUser);
            var token = new Token();
            _MockTokenBuilder.Setup(m => m.BuildAsync(creds, user))
                             .ReturnsAsync(token);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.AreEqual(token, result.Token);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_Valid_NotHashed_TrimWhitespace_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234 ";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = password };
            var user = new User { Id = 101, Username = username, Password = password.Trim(), Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            var role = new UserRole { Id = 1, Name = "TestRole", Enabled = true, Description = "This is a Test Role", LandingPageId = LandingPages.Admin };
            var odataRole = role.AsOdata<UserRole, int>();
            var related = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(UserRole) };
            related.Add(odataRole);
            odataUser.RelatedEntityCollection.Add(related);
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true))
                           .ReturnsAsync(odataUser);
            var token = new Token();
            _MockTokenBuilder.Setup(m => m.BuildAsync(creds, user))
                             .ReturnsAsync(token);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.AreEqual(token, result.Token);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_Valid_NotHashed_InvalidPassword_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234";
            var invalidPassword = "invalid";
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = invalidPassword };
            var user = new User { Id = 101, Username = username, Password = password, Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true))
                           .ReturnsAsync(odataUser);
            var token = new Token();

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_Valid_Hashed_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234";
            var salt = "abcdef";
            var hashedPassword = Hash.Get(password, salt);
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = password };
            var user = new User { Id = 101, Username = username, Password = hashedPassword, IsHashed = true, Salt = salt, Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            var role = new UserRole { Id = 1, Name = "TestRole", Enabled = true, Description = "This is a Test Role", LandingPageId = LandingPages.Admin };
            var odataRole = role.AsOdata<UserRole, int>();
            var related = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(UserRole) };
            related.Add(odataRole);
            odataUser.RelatedEntityCollection.Add(related);
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true))
                           .ReturnsAsync(odataUser);
            var token = new Token();
            _MockTokenBuilder.Setup(m => m.BuildAsync(creds, user))
                             .ReturnsAsync(token);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.AreEqual(token, result.Token);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_Valid_Hashed_TrimWhitespace_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234 ";
            var salt = "abcdef";
            var hashedPassword = Hash.Get(password.Trim(), salt);
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = password };
            var user = new User { Id = 101, Username = username, Password = hashedPassword, IsHashed = true, Salt = salt, Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            var role = new UserRole { Id = 1, Name = "TestRole", Enabled = true, Description = "This is a Test Role", LandingPageId = LandingPages.Admin };
            var odataRole = role.AsOdata<UserRole, int>();
            var related = new RelatedEntityCollection { Entity = nameof(UserRole), RelatedEntity = nameof(UserRole) };
            related.Add(odataRole);
            odataUser.RelatedEntityCollection.Add(related);

            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true))
                           .ReturnsAsync(odataUser);
            var token = new Token();
            _MockTokenBuilder.Setup(m => m.BuildAsync(creds, user))
                             .ReturnsAsync(token);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.AreEqual(token, result.Token);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UserCredentialsValidator_IsValidAsync_CredsHasUser_Valid_Hashed_InvalidPassword_Test()
        {
            // Arrange
            var username = "Jared";
            var password = "pw-1234";
            var invalidPassword = "invalid";
            var salt = "abcdef";
            var hashedPassword = Hash.Get(password.Trim(), salt);
            var userCredentialsValidator = CreateUserCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = invalidPassword };
            var user = new User { Id = 101, Username = username, Password = hashedPassword, IsHashed = true, Salt = salt, Enabled = true };
            var odataUser = user.AsOdata<User, long>();
            _MockUserClient.Setup(m => m.GetByAlternateKeyAsync(username, "?$expand=UserRole", true))
                           .ReturnsAsync(odataUser);
            var token = new Token();

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
