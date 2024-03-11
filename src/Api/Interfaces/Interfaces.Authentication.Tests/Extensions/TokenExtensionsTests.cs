using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Extensions
{
    [TestClass]
    public class TokenExtensionsTests
    {
        #region ToConcrete
        [TestMethod]
        public void TokenExtensions_ToConcrete_IEnumerableToken_Works()
        {
            // Arrange
            IToken token = new FakeToken { Text = "FakeToken" };
            var tokens = new List<IToken> { token };

            // Act
            var result = tokens.ToConcrete<Token>();

            // Assert
            Assert.AreEqual(typeof(List<Token>), result.GetType());
        }
        #endregion

        #region ToConcrete
        [TestMethod]
        public void TokenExtensions_ToConcrete_SingleToken_Works()
        {
            // Arrange
            IToken token = new FakeToken { Text = "FakeToken" };

            // Act
            var result = token.ToConcrete<Token>();

            // Assert
            Assert.AreEqual(typeof(Token), result.GetType());
        }
        #endregion

        #region GetClaimValues
        [TestMethod]
        public void TokenExtensions_GetClaimValues_TokenNull_Test()
        {
            // Arrange
            IToken token = null;
            string domainSubject = "SubjectY";
            string claimName = "NameY";

            // Act
            var result = token.GetClaimValue(domainSubject, claimName);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimDomain))]
        public void TokenExtensions_GetClaimValues_ClaimDomainsNullOrEmpty_Test(List<ClaimDomain> claimDomains)
        {
            // Arrange
            IToken token = new FakeToken { Text = "FakeToken", ClaimDomains = claimDomains };
            string domainSubject = "SubjectY";
            string claimName = "NameY";

            // Act
            var result = token.GetClaimValue(domainSubject, claimName);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimsList))]
        public void TokenExtensions_GetClaimValues_ClaimNullOrEmpty_Test(ClaimsList claims)
        {
            // Arrange
            string domainSubject = "SubjectY";
            string claimName = "NameY";
            var claimDomain = new ClaimDomain { Subject = domainSubject, Claims = claims };
            IToken token = new FakeToken { Text = "FakeToken", ClaimDomains = new List<ClaimDomain> { claimDomain } };

            // Act
            var result = token.GetClaimValue(domainSubject, claimName);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TokenExtensions_GetClaimValues_ClaimHasValue_Test()
        {
            // Arrange
            string domainSubject = "SubjectY";
            string claimName = "NameY";
            string claimValue = "ValueY";
            var claim = new Claim { Name = claimName, Value = claimValue };
            var claimDomain = new ClaimDomain { Subject = domainSubject };
            claimDomain.Claims.Add(claim);
            IToken token = new FakeToken { Text = "FakeToken", ClaimDomains = new List<ClaimDomain> { claimDomain } };

            // Act
            var result = token.GetClaimValue(domainSubject, claimName);

            // Assert
            Assert.AreEqual(claimValue, result);
        }
        #endregion

        #region GetClaimValues<T>
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void TokenExtensions_GetClaimValuesT_ClaimHasNullEmptyOrWhitespaceValue_Test(string claimValue)
        {
            // Arrange
            string domainSubject = "SubjectY";
            string claimName = "NameY";
            var claim = new Claim { Name = claimName, Value = claimValue };
            var claimDomain = new ClaimDomain { Subject = domainSubject };
            claimDomain.Claims.Add(claim);
            IToken token = new FakeToken { Text = "FakeToken", ClaimDomains = new List<ClaimDomain> { claimDomain } };

            // Act
            var result = token.GetClaimValue<int>(domainSubject, claimName);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TokenExtensions_GetClaimValuesT_ClaimHasValue_Test()
        {
            // Arrange
            string domainSubject = "SubjectY";
            string claimName = "NameY";
            string claimValue = "100";
            var claim = new Claim { Name = claimName, Value = claimValue };
            var claimDomain = new ClaimDomain { Subject = domainSubject };
            claimDomain.Claims.Add(claim);
            IToken token = new FakeToken { Text = "FakeToken", ClaimDomains = new List<ClaimDomain> { claimDomain } };

            // Act
            var result = token.GetClaimValue<int>(domainSubject, claimName);

            // Assert
            Assert.AreEqual(100, result);
        }
        #endregion
    }
}