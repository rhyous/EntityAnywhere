﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests
{
    [TestClass]
    public class TokenHeaderValidatorTests
    {
        private MockRepository _MockRepository;

        private Mock<ITokenDecoder> _MockTokenDecoder;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IEntityNameProvider> _MockEntityNameProvider;
        private Mock<IEntityPermissionChecker> _MockEntityPermissionChecker;
        private Mock<IHeadersUpdater> _MockHeadersUpdater;
        private Mock<ICustomCustomerRoleAuthorization> _MockCustomCustomerRoleAuthorization;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockTokenDecoder = _MockRepository.Create<ITokenDecoder>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockEntityNameProvider = _MockRepository.Create<IEntityNameProvider>();
            _MockEntityPermissionChecker = _MockRepository.Create<IEntityPermissionChecker>();
            _MockHeadersUpdater = _MockRepository.Create<IHeadersUpdater>();
            _MockCustomCustomerRoleAuthorization = _MockRepository.Create<ICustomCustomerRoleAuthorization>();
        }

        private TokenHeaderValidator CreateTokenHeaderValidator()
        {
            return new TokenHeaderValidator(
                _MockTokenDecoder.Object,
                _MockAppSettings.Object,
                _MockEntityNameProvider.Object,
                _MockEntityPermissionChecker.Object,
                _MockHeadersUpdater.Object,
                _MockCustomCustomerRoleAuthorization.Object);
        }

        #region Headers
        [TestMethod]
        public void TokenHeaderValidator_Headers_Property()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            var nameValueCollection = new NameValueCollection();

            // Act
            var result = headerValidator.Headers;

            // Assert
            Assert.AreEqual("Token", result[0]);

        }
        #endregion


        #region User
        [TestMethod]
        public void TokenHeaderValidator_UserId_Property()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();

            // Act
            headerValidator.UserId = 10;
            var result = headerValidator.UserId;

            // Assert
            Assert.AreEqual(10, result);

        }
        #endregion

        #region IsValidAsync
        [TestMethod]
        public async Task TokenHeaderValidator_IsValidAsync_GivenNullTokenText_ReturnsFalse()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            var nameValueCollection = new NameValueCollection();

            // Act
            var result = await headerValidator.IsValidAsync(nameValueCollection);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task TokenHeaderValidator_IsValidAsync_TokenDecoder_ReturnsNullToken_False()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            var nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("Token", "TokenValueTest");
            nameValueCollection.Add("Text", "TokenTextTest");
            nameValueCollection.Add("AbsolutePath", "/AddendumService.svc/Addenda");


            _MockTokenDecoder.Setup(m => m.Decode(It.IsAny<string>()))
                             .Returns<Token>(null);

            // Act
            var result = await headerValidator.IsValidAsync(nameValueCollection);

            // Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public async Task TokenHeaderValidator_IsValidAsync_TokenDecoder_Expired_False()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            var headers = new NameValueCollection();
            headers.Add("Token", "TokenValueTest");
            headers.Add("Text", "TokenTextTest");
            var absolutePath = "/AddendumService.svc/Addenda";
            headers.Add("AbsolutePath", absolutePath);

            var token = new Token
            {
                Text = "SomeFakeToken",
                CreateDate = DateTimeOffset.Now.AddDays(-7)
            };
            _MockTokenDecoder.Setup(m => m.Decode(It.IsAny<string>()))
                             .Returns(token);

            _MockHeadersUpdater.Setup(m => m.Update(token, headers));

            var entity = "Addendum";
            _MockEntityNameProvider.Setup(m => m.Provide(absolutePath)).Returns(entity);

            var nvcAppSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            // Act
            var result = await headerValidator.IsValidAsync(headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task TokenHeaderValidator_IsValidAsync_GivenValidParameters_EntityPermissionChecker_ReturnsTrue()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            var url = "/AddendumService.svc/Addenda";
            var userId = 127001234;

            var nvcHeaders = new NameValueCollection
            {
                { "Token", "TokenValueTest" },
                { "AbsolutePath", url },
                { "HttpMethod", "GET" }
            };
            _MockHeadersUpdater.Setup(m => m.Update(It.IsAny<IToken>(), nvcHeaders));

            var nvcAppSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            var collection = new OdataObjectCollection<Token, long>();
            var token = new Token
            {
                Text = "TokenValueTest",
                CreateDate = DateTimeOffset.Now,
                CredentialEntity = "User",
                CredentialEntityId = userId,
                RoleId = 2,
                Role = "Customer",
                ClaimDomains = new List<ClaimDomain>
                {
                }
            };
            _MockTokenDecoder.Setup(m => m.Decode(It.IsAny<string>()))
                             .Returns(token);

            var entity = "Addendum";
            _MockEntityNameProvider.Setup(m => m.Provide(url)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(token.RoleId, entity)).Returns(true);

            // Act
            var result = await headerValidator.IsValidAsync(nvcHeaders);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TokenHeaderValidator_IsValidAsync_GivenValidParameters_CustomCustomerRoleAuthorization_ReturnsTrue()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            var sapId = "9900123456";
            var url = "/AddendumService.svc/Addenda";
            var userId = 127001234;

            var nvcHeaders = new NameValueCollection
            {
                { "Token", "TokenValueTest" },
                { "AbsolutePath", url },
                { "HttpMethod", "GET" }
            };
            _MockHeadersUpdater.Setup(m => m.Update(It.IsAny<IToken>(), nvcHeaders));

            var nvcAppSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            var role = "Customer";
            var roleId = 2;
            var collection = new OdataObjectCollection<Token, long>();
            var token = new Token
            {
                Text = "TokenValueTest",
                CreateDate = DateTimeOffset.Now,
                CredentialEntity = "User",
                CredentialEntityId = userId,
                Role = role,
                RoleId = roleId,
                ClaimDomains = new List<ClaimDomain>
                {
                    new ClaimDomain
                    {
                        Subject = "Organization",
                        Claims = new ClaimsList
                        {
                            new Claim
                            {
                                Domain = new ClaimDomain(),
                                Subject = "Organization",
                                Issuer = "testIssuer",
                                Name = "SapId",
                                Value = sapId,
                                ValueType = "testValueType"
                            }
                        },
                        Issuer = "testIssuer",
                        OriginalIssuer = "testOriginalIssuer",
                    }
                }
            };
            _MockTokenDecoder.Setup(m => m.Decode(It.IsAny<string>()))
                             .Returns(token);

            var entity = "Addendum";
            _MockEntityNameProvider.Setup(m => m.Provide(url)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(token.RoleId, entity)).Returns(false);

            _MockCustomCustomerRoleAuthorization.Setup(m => m.IsAuthorized(nvcHeaders, token.RoleId))
                                                .Returns(true);

            // Act
            var result = await headerValidator.IsValidAsync(nvcHeaders);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion

        #region IsTokenVerified

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_Token_Null_False()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            IToken token = null;
            var headers = new NameValueCollection();

            // Act
            var result = headerValidator.IsTokenVerified(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_Headers_Null_False()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            IToken token = new Token();
            NameValueCollection headers = null;

            // Act
            var result = headerValidator.IsTokenVerified(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_Headers_AbsolutePath_Missing_False()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();
            IToken token = new Token();
            var headers = new NameValueCollection();

            // Act
            var result = headerValidator.IsTokenVerified(token, headers);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_EntityPermissionChecker_True()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();

            var orgSapIdClaim = new Claim { Subject = "Organization", Name = "SapId", Value = "9900001027" };
            var orgClaimDomain = new ClaimDomain { Subject = "Organization" };
            orgClaimDomain.Claims.Add(orgSapIdClaim);

            IToken token = new Token { ClaimDomains = new List<ClaimDomain> { orgClaimDomain }, RoleId = 10 };
            var absolutePath = "/MovieService/Movies";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath } };
            _MockHeadersUpdater.Setup(m => m.Update(token, headers));

            var entity = "Movie";
            _MockEntityNameProvider.Setup(m => m.Provide(absolutePath)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(token.RoleId, entity)).Returns(true);

            // Act
            var result = headerValidator.IsTokenVerified(token, headers);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TokenHeaderValidator_IsTokenVerified_CustomCustomerRoleAuthorization_True()
        {
            // Arrange
            var headerValidator = CreateTokenHeaderValidator();

            var role = "MyRole";
            var roleId = 10;
            var userRoleClaim = new Claim { Subject = "UserRole", Name = "Role", Value = role };
            var userRoleClaimDomain = new ClaimDomain { Subject = "UserRole" };
            userRoleClaimDomain.Claims.Add(userRoleClaim);

            IToken token = new Token { RoleId = roleId };
            var absolutePath = "/MovieService/Movies";
            var headers = new NameValueCollection { { "AbsolutePath", absolutePath } };
            _MockHeadersUpdater.Setup(m => m.Update(token, headers));

            var entity = "Movie";
            _MockEntityNameProvider.Setup(m => m.Provide(absolutePath)).Returns(entity);

            _MockEntityPermissionChecker.Setup(m => m.HasPermission(roleId, entity)).Returns(true);

            _MockCustomCustomerRoleAuthorization.Setup(m => m.IsAuthorized(headers, token.RoleId))
                                                .Returns(true);

            // Act
            var result = headerValidator.IsTokenVerified(token, headers);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion
    }
}
