using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    [TestClass]
    public class ClaimsProviderTests
    {
        private MockRepository _MockRepository;

        private Mock<IHeaders> _MockHeaders;
        private Mock<IJWTToken> _MockJWTToken;
        private Mock<IAppSettings> _MockAppSettings;
        private Mock<IAdminClaimsProvider> _MockAdminClaimsProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHeaders = _MockRepository.Create<IHeaders>();
            _MockJWTToken = _MockRepository.Create<IJWTToken>();
            _MockAppSettings = _MockRepository.Create<IAppSettings>();
            _MockAdminClaimsProvider = _MockRepository.Create<IAdminClaimsProvider>();
        }

        private ClaimsProvider CreateProvider()
        {
            return new ClaimsProvider(
                _MockHeaders.Object,
                _MockJWTToken.Object,
                _MockAppSettings.Object,
                _MockAdminClaimsProvider.Object);
        }

        #region GetClaims
        [TestMethod]
        public void ClaimsProvider_GetClaims_Success()
        {
            // Arrange
            var provider = CreateProvider();

            var token = "Token Value";
            var headers = new NameValueCollection { { "Token", token } };
            _MockHeaders.Setup(h => h.Collection).Returns(headers);

            var decodedToken = "Decoded Token";
            _MockJWTToken.Setup(t => t.DecodeToken(token)).Returns(decodedToken);

            var expected = new List<ClaimDomain>();
            _MockJWTToken.Setup(t => t.GetClaimDomains(decodedToken)).Returns(expected);

            // Act
            var actual = provider.GetClaims();

            // Assert
            Assert.AreEqual(expected, actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsProvider_GetClaims_TokenIsNull()
        {
            // Arrange
            var provider = CreateProvider();

            string token = null;
            var headers = new NameValueCollection { { "Token", token } };
            _MockHeaders.Setup(h => h.Collection).Returns(headers);
            var appSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);

            // Act
            var actual = provider.GetClaims();

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsProvider_GetClaims_TokenIsWhitespace()
        {
            // Arrange
            var provider = CreateProvider();

            var token = "   ";
            var headers = new NameValueCollection { { "Token", token } };
            _MockHeaders.Setup(h => h.Collection).Returns(headers);
            var appSettings = new NameValueCollection();
            _MockAppSettings.Setup(m => m.Collection).Returns(appSettings);

            // Act
            var actual = provider.GetClaims();

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsProvider_GetClaims_DecodedTokenIsNull()
        {
            // Arrange
            var provider = CreateProvider();

            var token = "Token Value";
            var headers = new NameValueCollection { { "Token", token } };
            _MockHeaders.Setup(h => h.Collection).Returns(headers);

            string decodedToken = null;
            _MockJWTToken.Setup(t => t.DecodeToken(token)).Returns(decodedToken);

            // Act
            // Assert
            Assert.ThrowsException<Exception>(() => provider.GetClaims());
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsProvider_GetClaims_NoToken_AdminToken_Invalid()
        {
            // Arrange
            var provider = CreateProvider();

            var invalidEntityAdminToken = "abc1234";
            var headers = new NameValueCollection { { "EntityAdminToken", invalidEntityAdminToken } };
            _MockHeaders.Setup(h => h.Collection).Returns(headers);
            var validEntityAdminToken = "abc1234-a";
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", validEntityAdminToken } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);

            // Act
            var actual = provider.GetClaims();

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsProvider_GetClaims_NoToken_AdminToken_Valid()
        {
            // Arrange
            var provider = CreateProvider();

            var validEntityAdminToken = "abc1234-a";
            var headers = new NameValueCollection { { "EntityAdminToken", validEntityAdminToken } };
            _MockHeaders.Setup(h => h.Collection).Returns(headers);
            var nvcAppSettings = new NameValueCollection { { "EntityAdminToken", validEntityAdminToken } };
            _MockAppSettings.Setup(m => m.Collection).Returns(nvcAppSettings);
            var claimDomains = new List<ClaimDomain>();
            _MockAdminClaimsProvider.Setup(m => m.ClaimDomains).Returns(claimDomains);

            // Act
            var actual = provider.GetClaims();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetClaim

        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimDomain))]
        public void ClaimsProvider_GetClaim_ClaimsAreNullOrEmpty(List<ClaimDomain> claimDomains)
        {
            // Arrange
            var provider = CreateProvider();
            provider.GetClaimDomainsMethod = () => claimDomains;
            var claimSubject = "Organization";
            var claimName = "Id";

            // Act
            // Assert
            Assert.ThrowsException<Exception>(() => provider.GetClaim(claimSubject, claimName));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimsList))]
        public void ClaimsProvider_GetClaim_OrgClaimDomainClaimsIsNullOrEmpty(ClaimsList list)
        {
            // Arrange
            var provider = CreateProvider();
            var claimSubject = "Organization";
            var claimName = "Id";
            var orgClaim = new ClaimDomain { Claims = list, Subject = claimSubject };
            var claimDomains = new List<ClaimDomain> { orgClaim };
            provider.ClaimDomains = claimDomains;

            // Act
            // Assert
            Assert.ThrowsException<Exception>(() => provider.GetClaim(claimSubject, claimName));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsProvider_GetClaim_OrgIdIsZero()
        {
            // Arrange
            var provider = CreateProvider();
            var claimSubject = "Organization";
            var claimName = "Id";

            var orgColorClaim = new Claim { Name = "FavoriteColor", Value = "Blue" };
            var orgClaimList = new ClaimsList { orgColorClaim };
            var orgClaimDomain = new ClaimDomain { Claims = orgClaimList, Subject = claimSubject };
            var claimDomains = new List<ClaimDomain> { orgClaimDomain };
            provider.ClaimDomains = claimDomains;

            // Act
            // Assert
            Assert.ThrowsException<Exception>(() => provider.GetClaim(claimSubject, claimName));
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetClaim<T>
        [TestMethod]
        public void ClaimsProvider_GetClaim_Success()
        {
            // Arrange
            var provider = CreateProvider();
            var claimSubject = "Organization";
            var claimName = "Id";
            var orgId = 5;

            var orgIdClaim = new Claim { Name = claimName, Value = orgId.ToString() };
            var orgClaimList = new ClaimsList { orgIdClaim };
            var orgClaimDomain = new ClaimDomain { Claims = orgClaimList, Subject = claimSubject };
            var claimDomains = new List<ClaimDomain> { orgClaimDomain };
            provider.ClaimDomains = claimDomains;


            // Act
            var actual = provider.GetClaim<int>(claimSubject, claimName);

            // Assert
            Assert.AreEqual(orgId, actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void ClaimsProvider_GetClaim_Null_ReturnsDefaultT(string orgIdAsString)
        {
            // Arrange
            var provider = CreateProvider();
            var claimSubject = "Organization";
            var claimName = "Id";

            var orgIdClaim = new Claim { Name = claimName, Value = orgIdAsString };
            var orgClaimList = new ClaimsList { orgIdClaim };
            var orgClaimDomain = new ClaimDomain { Claims = orgClaimList, Subject = claimSubject };
            var claimDomains = new List<ClaimDomain> { orgClaimDomain };
            provider.ClaimDomains = claimDomains;


            // Act
            var actual = provider.GetClaim<int>(claimSubject, claimName);

            // Assert
            Assert.AreEqual(0, actual);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
