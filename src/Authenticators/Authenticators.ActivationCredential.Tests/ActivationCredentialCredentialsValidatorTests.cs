using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.UnitTesting;
using Rhyous.WebFramework.Authenticators;
using Rhyous.WebFramework.Clients2;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Authenticators.Tests
{
    [TestClass]
    public class ActivationCredentialCredentialsValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<IAdminEntityClientAsync<ActivationCredential, long>> _MockAdminActivationCredentialClientAsync;
        private Mock<IPasswordSecurity> _MockPasswordSecurity;
        private Mock<IClaimsBuilder> _MockClaimsBuilder;
        private Mock<IJWTToken> _MockJWTToken;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAdminActivationCredentialClientAsync = _MockRepository.Create<IAdminEntityClientAsync<ActivationCredential, long>>();
            _MockPasswordSecurity = _MockRepository.Create<IPasswordSecurity>();
            _MockClaimsBuilder = _MockRepository.Create<IClaimsBuilder>();
            _MockJWTToken = _MockRepository.Create<IJWTToken>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private ActivationCredentialCredentialsValidator CreateActivationCredentialCredentialsValidator()
        {
            return new ActivationCredentialCredentialsValidator(
                _MockAdminActivationCredentialClientAsync.Object,
                _MockPasswordSecurity.Object,
                _MockClaimsBuilder.Object,
                _MockJWTToken.Object,
                _MockLogger.Object);
        }

        #region IsValidAsync
        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_Creds_Null_Throws()
        {
            // Arrange
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = null;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await activationCredentialCredentialsValidator.IsValidAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_CredsDotUser_NullEmptyOrWhitespace_Throws(string user)
        {
            // Arrange
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = user, Password = "pw123" };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await activationCredentialCredentialsValidator.IsValidAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_CredsDotPassword_NullEmptyOrWhitespace_Throws(string pw)
        {
            // Arrange
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = "user27", Password = pw };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await activationCredentialCredentialsValidator.IsValidAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_CredsHasUser_ReturnsNull_Test()
        {
            // Arrange
            var username = "Jared";
            var userCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = "pw-abc" };
            OdataObject<ActivationCredential, long> odataUser = null;
            _MockAdminActivationCredentialClientAsync.Setup(m => m.GetByAlternateKeyAsync(username, true))
                                                     .ReturnsAsync(odataUser);

            // Act
            var result = await userCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_ActivationCredentials_Disabled()
        {
            // Arrange
            var username = "Jared";
            var pw = "somePass123";
            var encryptedPassword = "***{pw}***";
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = pw };
            var activationCredential = new ActivationCredential { Username = username, Password = encryptedPassword, Enabled = false, OrganizationId = 1027 };
            var odataActivationCredential = activationCredential.AsOdata<ActivationCredential, long>();
            _MockAdminActivationCredentialClientAsync.Setup(m => m.GetByAlternateKeyAsync(username, true))
                                                     .ReturnsAsync(odataActivationCredential);

            // Act
            var result = await activationCredentialCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("This ActivationCredentials user 'Jared' is disabled.", result.Message);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_BadPassword()
        {
            // Arrange
            var username = "Jared";
            var pw = "somePass123"; // Forgot the last 4
            var encryptedPassword = $"***{pw}4***";
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = pw };
            var activationCredential = new ActivationCredential { Username = username, Password = encryptedPassword, Enabled = true, OrganizationId = 1027 };
            var odataActivationCredential = activationCredential.AsOdata<ActivationCredential, long>();
            _MockAdminActivationCredentialClientAsync.Setup(m => m.GetByAlternateKeyAsync(username, true))
                                                     .ReturnsAsync(odataActivationCredential);

            _MockPasswordSecurity.Setup(m => m.Compare(pw, encryptedPassword)).Returns(false);

            // Act
            var result = await activationCredentialCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNull(result.Token);
            Assert.IsFalse(result.Success);
            Assert.AreEqual($"The password provided for ActivationCredentials user '{username}' is incorrect.", result.Message);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_Valid()
        {
            // Arrange
            var username = "Jared";
            var pw = "somePass123";
            var encryptedPassword = $"***{pw}***";
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = pw };
            var activationCredential = new ActivationCredential { Username = username, Password = encryptedPassword, Enabled = true, OrganizationId = 1027 };
            var odataActivationCredential = activationCredential.AsOdata<ActivationCredential, long>();
            _MockAdminActivationCredentialClientAsync.Setup(m => m.GetByAlternateKeyAsync(username, true))
                                                     .ReturnsAsync(odataActivationCredential);

            _MockPasswordSecurity.Setup(m => m.Compare(pw, encryptedPassword)).Returns(true);

            var listClaimDomains = new List<ClaimDomain>();
            _MockClaimsBuilder.Setup(m => m.BuildAsync(activationCredential))
                              .ReturnsAsync(listClaimDomains);
            var tokenText = "abc-token";
            _MockJWTToken.Setup(m => m.GetTokenText(listClaimDomains))
                         .Returns(tokenText);

            // Act
            var result = await activationCredentialCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNotNull(result.Token);
            Assert.AreEqual(listClaimDomains, result.Token.ClaimDomains);
            Assert.AreEqual(tokenText, result.Token.Text);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_InputPasswordHasTrailingSpace_StoredDoesNot_Valid()
        {
            // Arrange
            var username = "Jared";
            var pw = "somePass123 "; // Notice the trailing space
            var encryptedPasswordwithSpace = $"***{pw}***";
            var encryptedPasswordNoSpace = $"***{pw.Trim()}***";
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = pw };
            var activationCredential = new ActivationCredential { Username = username, Password = encryptedPasswordNoSpace, Enabled = true, OrganizationId = 1027 };
            var odataActivationCredential = activationCredential.AsOdata<ActivationCredential, long>();
            _MockAdminActivationCredentialClientAsync.Setup(m => m.GetByAlternateKeyAsync(username, true))
                                                     .ReturnsAsync(odataActivationCredential);

            _MockPasswordSecurity.Setup(m => m.Compare(pw, encryptedPasswordNoSpace)).Returns(false);
            _MockPasswordSecurity.Setup(m => m.Compare(pw.Trim(), encryptedPasswordNoSpace)).Returns(true);

            var listClaimDomains = new List<ClaimDomain>();
            _MockClaimsBuilder.Setup(m => m.BuildAsync(activationCredential))
                              .ReturnsAsync(listClaimDomains);
            var tokenText = "abc-token";
            _MockJWTToken.Setup(m => m.GetTokenText(listClaimDomains))
                         .Returns(tokenText);

            // Act
            var result = await activationCredentialCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNotNull(result.Token);
            Assert.AreEqual(listClaimDomains, result.Token.ClaimDomains);
            Assert.AreEqual(tokenText, result.Token.Text);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ActivationCredentialCredentialsValidator_IsValidAsync_InputPasswordHasTrailingSpace_StoredHasSpaceToo_Valid()
        {
            // Arrange
            var username = "Jared";
            var pw = "somePass123 "; // Notice the trailing space
            var encryptedPasswordwithSpace = $"***{pw}***";
            var activationCredentialCredentialsValidator = CreateActivationCredentialCredentialsValidator();
            ICredentials creds = new Credentials { User = username, Password = pw };
            var activationCredential = new ActivationCredential { Username = username, Password = encryptedPasswordwithSpace, Enabled = true, OrganizationId = 1027 };
            var odataActivationCredential = activationCredential.AsOdata<ActivationCredential, long>();
            _MockAdminActivationCredentialClientAsync.Setup(m => m.GetByAlternateKeyAsync(username, true))
                                                     .ReturnsAsync(odataActivationCredential);

            _MockPasswordSecurity.Setup(m => m.Compare(pw, encryptedPasswordwithSpace)).Returns(true);

            var listClaimDomains = new List<ClaimDomain>();
            _MockClaimsBuilder.Setup(m => m.BuildAsync(activationCredential))
                              .ReturnsAsync(listClaimDomains);
            var tokenText = "abc-token";
            _MockJWTToken.Setup(m => m.GetTokenText(listClaimDomains))
                         .Returns(tokenText);

            // Act
            var result = await activationCredentialCredentialsValidator.IsValidAsync(creds);

            // Assert
            Assert.IsNotNull(result.Token);
            Assert.AreEqual(listClaimDomains, result.Token.ClaimDomains);
            Assert.AreEqual(tokenText, result.Token.Text);
            Assert.IsTrue(result.Success);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
