using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Services;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Rhyous.EntityAnywhere.Exceptions;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Tools
{
    [TestClass]
    public class JWTTokenTests
    {
        private MockRepository _MockRepository;

        private Mock<ITokenKeyPair> _MockTokenKeyPair;

        private string JwtTokenWithEmptyClaims = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.W10.bXNk0ns2eoCNwo5DbY_BDfu42tlkhp_kjUYd5PwpBZvoUuZl8ERyXxqilhwFO3tViLZvMh7m2MI8gtPzSHYduN5jNXzxnCTKMhAARd9Up_mabQXQbp2gBVcNbC0U3dvrVdq0tllmfUzGk3hGzRVhSKdRdCMlec7aWZ7SSfd60GhqIhrO3lFk9Ln_fSrsGE3RskzNXhd1vswTu2uQWs94BCtNYBfz1-eZdyD-bjlvmDkbF7cSHkMJvO27qGHTE0mXCTm2XmhAfFeSlpkHMjjTv12vIQgH78mOXC6i_eSRL1XPoNplxt9oTCigKW1U6k3Sim3JTbFGXMHIapa-1cnnGA";
        private string JwtTokenWithSampleClaims = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.W3siU3ViamVjdCI6IlVzZXIiLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkiLCJDbGFpbXMiOlt7Ik5hbWUiOiJVc2VybmFtZSIsIlZhbHVlIjoiRnVsYW5vLkRlVGFsQGRvbWFpbi50bGQiLCJTdWJqZWN0IjoiVXNlciIsIklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSJ9LHsiTmFtZSI6IklkIiwiVmFsdWUiOiI5OTAxMjM0NTYiLCJTdWJqZWN0IjoiVXNlciIsIklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSJ9LHsiTmFtZSI6Ikxhc3RBdXRoZW50aWNhdGVkIiwiVmFsdWUiOiJUdWUsIDAyIE5vdiAyMDIxIDIwOjMyOjI2IEdNVCIsIlN1YmplY3QiOiJVc2VyIiwiSXNzdWVyIjoiTE9DQUwgQVVUSE9SSVRZIn1dfSx7IlN1YmplY3QiOiJPcmdhbml6YXRpb24iLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkiLCJDbGFpbXMiOlt7Ik5hbWUiOiJJZCIsIlZhbHVlIjoiOTkwMDAxMDI3IiwiU3ViamVjdCI6Ik9yZ2FuaXphdGlvbiIsIklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSJ9LHsiTmFtZSI6Ik5hbWUiLCJWYWx1ZSI6IkNvb2xpbyBDdXN0b21lciIsIlN1YmplY3QiOiJPcmdhbml6YXRpb24iLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkifSx7Ik5hbWUiOiJDYXRlZ29yeSIsIlZhbHVlIjoiNCIsIlN1YmplY3QiOiJPcmdhbml6YXRpb24iLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkifV19LHsiU3ViamVjdCI6IlVzZXJSb2xlIiwiSXNzdWVyIjoiTE9DQUwgQVVUSE9SSVRZIiwiQ2xhaW1zIjpbeyJOYW1lIjoiUm9sZSIsIlZhbHVlIjoiQ3VzdG9tZXIiLCJTdWJqZWN0IjoiVXNlclJvbGUiLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkifV19XQ.d6mqIHw3nYNEav9unGMEde8jzLkUpJpsaYp0Zxy9CW0kwu-VVNJNcV_Et-Fwjca7QLx_bvexslDfDnVxAuaTjGBZYgAkDy8WcxbjmmxR4PWp1un3Av6O02dtyVZyrL-zw4ANq80b5rPWS_dlKZdzi_j9vK9K8V-6IHndEvDO7ds_i7NYWQ71kSqlkPJYb8EPA-os4SgaAdsmmxFQGXG785y9AhIUKfHtOunQzjXSGhi7D4jsOAT7UB5i3lhcGa22LpMFtwyJtT9HnZG9u3egEtOpJIsKwaEmt_fTh2kJv1ObdvjwAwdwmbQMAbymmw68xV-A5ObnPmjsHhHXqfCrPQ";

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockTokenKeyPair = _MockRepository.Create<ITokenKeyPair>();
        }

        private JWTToken CreateJWTToken()
        {
            return new JWTToken(_MockTokenKeyPair.Object);
        }

        #region GetTokenText
        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimDomain))]
        public void JWTToken_GetTokenText_EmptyClaimDomains_Test(List<ClaimDomain> claimDomains)
        {
            // Arrange
            var expectedJwt = JwtTokenWithEmptyClaims;
            var jWTToken = CreateJWTToken();
            var privateKey = File.ReadAllText(@"Data/Keys/PrivateKey.pem");
            _MockTokenKeyPair.Setup(m => m.PrivateKey).Returns(privateKey);

            // Act
            var result = jWTToken.GetTokenText(claimDomains);

            // Assert
            Assert.AreEqual(expectedJwt, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void JWTToken_GetTokenText_SampleClaimDomains_Test()
        {
            // Arrange
            var expectedJwt = JwtTokenWithSampleClaims;
            var jWTToken = CreateJWTToken();
            var privateKey = File.ReadAllText(@"Data/Keys/PrivateKey.pem");
            _MockTokenKeyPair.Setup(m => m.PrivateKey).Returns(privateKey);
            var claimsJson = File.ReadAllText(@"Data/SampleClaims.json");
            var claimDomains = JsonConvert.DeserializeObject<List<ClaimDomain>>(claimsJson);

            // Act
            var result = jWTToken.GetTokenText(claimDomains);

            // Assert
            Assert.AreEqual(expectedJwt, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimDomain))]
        public void JWTToken_GetTokenText_BadKey_Test(List<ClaimDomain> claimDomains)
        {
            // Arrange
            var expectedJwt = JwtTokenWithEmptyClaims;
            var jWTToken = CreateJWTToken();
            // Notice below, it is the public key, the wrong one. This is intential for this test.
            var privateKey = File.ReadAllText(@"Data/Keys/PublicKey.pem"); 
            _MockTokenKeyPair.Setup(m => m.PrivateKey).Returns(privateKey);

            // Act & Assert
            Assert.ThrowsException<ConfigurationException>(() =>
            {
                var result = jWTToken.GetTokenText(claimDomains);
            });
            _MockRepository.VerifyAll();
        }
        #endregion

        #region CreateToken
        [TestMethod]
        [ListTNullOrEmpty(typeof(ClaimDomain))]
        public void JWTToken_CreateToken_EmptyClaimDomains_Test(List<ClaimDomain> claimDomains)
        {
            // Arrange
            var expectedJwt = JwtTokenWithEmptyClaims;
            var jWTToken = CreateJWTToken();
            var privateKey = File.ReadAllText(@"Data/Keys/PrivateKey.pem");

            // Act
            var result = jWTToken.CreateToken(claimDomains, privateKey);

            // Assert
            Assert.AreEqual(expectedJwt, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void JWTToken_CreateToken_SampleClaimDomains_Test()
        {
            // Arrange
            var expectedJwt = JwtTokenWithSampleClaims;
            var jWTToken = CreateJWTToken();
            var privateKey = File.ReadAllText(@"Data/Keys/PrivateKey.pem");
            var claimsJson = File.ReadAllText(@"Data/SampleClaims.json");
            var claimDomains = JsonConvert.DeserializeObject<List<ClaimDomain>>(claimsJson);

            // Act
            var result = jWTToken.CreateToken(claimDomains, privateKey);

            // Assert
            Assert.AreEqual(expectedJwt, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region DecodeToken
        [TestMethod]
        public void JWTToken_DecodeToken_EmptyClaimDomains_Test()
        {
            // Arrange
            var jWTToken = CreateJWTToken();
            string token = JwtTokenWithEmptyClaims;
            var publicKey = File.ReadAllText(@"Data/Keys/PublicKey.pem");
            _MockTokenKeyPair.Setup(m => m.PublicKey).Returns(publicKey);

            // Act
            var result = jWTToken.DecodeToken(token);

            // Assert
            Assert.AreEqual("[]", result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void JWTToken_DecodeToken_SampleClaimDomains_Test()
        {
            // Arrange
            var jWTToken = CreateJWTToken();
            string token = JwtTokenWithSampleClaims;
            var publicKey = File.ReadAllText(@"Data/Keys/PublicKey.pem");
            _MockTokenKeyPair.Setup(m => m.PublicKey).Returns(publicKey);
            var claimsJson = File.ReadAllText(@"Data/SampleClaims.json");

            // Act
            var result = jWTToken.DecodeToken(token);

            // Assert
            Assert.AreEqual(claimsJson, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void JWTToken_DecodeToken_BadKey_Test()
        {
            // Arrange
            var jWTToken = CreateJWTToken();
            string token = JwtTokenWithEmptyClaims;
            // Notice below, it is the private key, the wrong one. This is intential for this test.
            var publicKey = File.ReadAllText(@"Data/Keys/PrivateKey.pem");
            _MockTokenKeyPair.Setup(m => m.PublicKey).Returns(publicKey);

            // Act & Assert
            Assert.ThrowsException<ConfigurationException>(() =>
            {
                var result = jWTToken.DecodeToken(token);
            });
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetClaimDomains
        [TestMethod]
        public void JWTToken_GetClaimDomains_SampleClaimDomains_Test()
        {
            // Arrange
            var jWTToken = CreateJWTToken();
            string decodedToken = File.ReadAllText(@"Data/SampleClaims.json");

            // Act
            var result = jWTToken.GetClaimDomains(decodedToken);

            // Assert
            Assert.AreEqual(3, result.Count);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
