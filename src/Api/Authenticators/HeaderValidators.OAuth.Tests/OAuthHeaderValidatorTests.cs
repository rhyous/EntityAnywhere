using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests
{
    [TestClass]
    public class OAuthHeaderValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<IBearerDecoder> _MockBearerDecoder;
        private Mock<IEntityNameProvider> _MockEntityNameProvider;
        private Mock<IEntityPermissionChecker> _MockEntityPermissionChecker;
        private Mock<IHeadersUpdater> _MockHeadersUpdater;
        private Mock<ICustomCustomerRoleAuthorization> _MockCustomCustomerRoleAuthorization;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockBearerDecoder = _MockRepository.Create<IBearerDecoder>();
            _MockEntityNameProvider = _MockRepository.Create<IEntityNameProvider>();
            _MockEntityPermissionChecker = _MockRepository.Create<IEntityPermissionChecker>();
            _MockHeadersUpdater = _MockRepository.Create<IHeadersUpdater>();
            _MockCustomCustomerRoleAuthorization = _MockRepository.Create<ICustomCustomerRoleAuthorization>();
        }

        private OAuthHeaderValidator CreateOAuthHeaderValidator()
        {
            return new OAuthHeaderValidator(
                _MockBearerDecoder.Object,
                _MockEntityNameProvider.Object,
                _MockEntityPermissionChecker.Object,
                _MockHeadersUpdater.Object,
                _MockCustomCustomerRoleAuthorization.Object);
        }

        #region Headers
        [TestMethod]
        public void OAuthHeaderValidator_Headers_Property()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var nameValueCollection = new NameValueCollection();

            // Act
            var result = headerValidator.Headers;

            // Assert
            Assert.AreEqual("Bearer", result[0]);

        }
        #endregion

        #region User
        [TestMethod]
        public void OAuthnHeaderValidator_UserId_Property()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();

            // Act
            headerValidator.UserId = 10;
            var result = headerValidator.UserId;

            // Assert
            Assert.AreEqual(10, result);

        }
        #endregion

        #region IsValidAsync
        [TestMethod]
        public void OAuthHeaderValidator_IsValidAsync_GivenNullTokenText_ReturnsFalse()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            mockHeaders.SetupGet(m => m["Bearer"]).Returns((string)null);

            // Act
            var result = headerValidator.IsValidAsync(mockHeaders.Object);
            // Assert
            Assert.AreEqual(result.Result, false);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void OAuthHeaderValidator_IsValidAsync_TokenDecoder_ReturnsNullToken_False()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            mockHeaders.SetupGet(m => m["Bearer"]).Returns("TokenValueTest");

            IAccessToken token = null;
            _MockBearerDecoder.Setup(m => m.DecodeAsync(It.IsAny<string>()))
                              .ReturnsAsync(token);
            // Act
            var result = headerValidator.IsValidAsync(mockHeaders.Object);
            // Assert
            Assert.AreEqual(result.Result, false);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task OAuthHeaderValidator_IsValidAsync_TokenDecoder_Expired_False()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            var tokenText = "TokenValueTest";
            mockHeaders.SetupGet(m => m["Bearer"]).Returns(tokenText);

            IAccessToken token = new AccessToken
            {
                Expires = DateTimeOffset.Now.AddDays(-7).ToUnixTimeSeconds()
            };
            _MockBearerDecoder.Setup(m => m.DecodeAsync(tokenText))
                              .ReturnsAsync(token);

            // Act
            var result = await headerValidator.IsValidAsync(mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task OAuthHeaderValidator_IsValidAsync_GivenValidParameters_EntityPermissionChecker_ReturnsTrue()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            var tokenText = "TokenValueTest";
            mockHeaders.SetupGet(m => m["Bearer"]).Returns(tokenText);
            var url = "/AddendumService.svc/Addenda";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(url);

            _MockHeadersUpdater.Setup(m => m.Update(It.IsAny<IAccessToken>(), mockHeaders.Object));

            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            var token = new AccessToken()
            {
                Audience = "me",
                AuthTime = 0L,
                ClientId = "1",
                Expires = (long)span.TotalSeconds + 100L,
                IdentityProvider = "eaf",
                Issuer = "EAF",
                NotBefore = 10L,
                Scope = new string[] { "nada" },
                Subject = "someone@acme.com",
                UserId = 1,
                UserRoleId = 2
            };
            _MockBearerDecoder.Setup(m => m.DecodeAsync(It.IsAny<string>()))
                              .ReturnsAsync(token);

            var entity = "Addendum";
            _MockEntityNameProvider.Setup(m => m.Provide(url)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(2, entity)).Returns(true);

            // Act
            var result = await headerValidator.IsValidAsync(mockHeaders.Object);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task OAuthHeaderValidator_IsValidAsync_GivenValidParameters_CustomCustomerRoleAuthorization_ReturnsTrue()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();

            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            var tokenText = "TokenValueTest";
            mockHeaders.SetupGet(m => m["Bearer"]).Returns(tokenText);
            var url = "/AddendumService.svc/Addenda";
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(url);

            _MockHeadersUpdater.Setup(m => m.Update(It.IsAny<IAccessToken>(), mockHeaders.Object));

            var span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            var token = new AccessToken()
            {
                Audience = "me",
                AuthTime = 0L,
                ClientId = "1",
                Expires = (long)span.TotalSeconds + 100L,
                IdentityProvider = "eaf",
                Issuer = "EAF",
                NotBefore = 10L,
                Scope = new string[] { "nada" },
                Subject = "someone@acme.com",
                UserId = 1,
                UserRoleId = 2
            };
            _MockBearerDecoder.Setup(m => m.DecodeAsync(It.IsAny<string>()))
                              .ReturnsAsync(token);

            var entity = "Addendum";
            _MockEntityNameProvider.Setup(m => m.Provide(url)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(2, entity)).Returns(false);

            _MockCustomCustomerRoleAuthorization.Setup(m=>m.IsAuthorized(token, mockHeaders.Object))
                                                .Returns(true);

            // Act
            var result = await headerValidator.IsValidAsync(mockHeaders.Object);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region IsTokenVerified

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_Token_Null_False()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            IAccessToken token = null;
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();

            // Act
            var result = headerValidator.IsTokenVerified(token, mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public void TokenHeaderValidator_IsTokenVerified_UserId_NotPositiveInt_False(int userRoleId)
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();

            IAccessToken token = new AccessToken { UserRoleId = userRoleId };
            
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();

            // Act
            var result = headerValidator.IsTokenVerified(token, mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_Headers_Null_False()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            IAccessToken token = new AccessToken();

            IHeadersContainer headers = null;

            // Act
            var result = headerValidator.IsTokenVerified(token, headers);

            // Assert
            Assert.IsFalse(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_Headers_AbsolutePath_Missing_False()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            IAccessToken token = new AccessToken { UserRoleId = 10 };
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns((string)null);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");

            // Act
            var result = headerValidator.IsTokenVerified(token, mockHeaders.Object);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_EntityPermissionChecker_True()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var roleId = 10;
            IAccessToken token = new AccessToken { UserRoleId = roleId };

            var absolutePath = "/MovieService/Movies";
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);
            mockHeaders.Setup(m => m.Get("HttpMethod")).Returns("GET");

            _MockHeadersUpdater.Setup(m => m.Update(token, mockHeaders.Object));

            var entity = "Movie";
            _MockEntityNameProvider.Setup(m => m.Provide(absolutePath)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(roleId, entity)).Returns(true);

            // Act
            var result = headerValidator.IsTokenVerified(token, mockHeaders.Object);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_CustomCustomerRoleAuthorization_True()
        {
            // Arrange
            var headerValidator = CreateOAuthHeaderValidator();
            var roleId = 2;

            IAccessToken token = new AccessToken { UserRoleId = roleId };
            var absolutePath = "/MovieService/Movies";
            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            mockHeaders.Setup(m => m.Get("AbsolutePath")).Returns(absolutePath);

            _MockHeadersUpdater.Setup(m => m.Update(token, mockHeaders.Object));

            var entity = "Movie";
            _MockEntityNameProvider.Setup(m => m.Provide(absolutePath)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(roleId, entity)).Returns(false);

            _MockCustomCustomerRoleAuthorization.Setup(m => m.IsAuthorized(token, mockHeaders.Object))
                                                .Returns(true);

            // Act
            var result = headerValidator.IsTokenVerified(token, mockHeaders.Object);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion
    }
}
