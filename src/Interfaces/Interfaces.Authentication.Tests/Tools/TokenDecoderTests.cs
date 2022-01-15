using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Tools
{
    [TestClass]
    public class TokenDecoderTests
    {
        private MockRepository _MockRepository;

        private Mock<IJWTToken> _MockJWTToken;
        private Mock<IUserRoleEntityDataCache> _MockUserRoleEntityDataCache;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockJWTToken = _MockRepository.Create<IJWTToken>();
            _MockUserRoleEntityDataCache = _MockRepository.Create<IUserRoleEntityDataCache>();
        }

        private TokenDecoder CreateTokenDecoder()
        {
            return new TokenDecoder(
                _MockJWTToken.Object,
                _MockUserRoleEntityDataCache.Object);
        }

        #region Decode

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void TokenDecoder_Decode_TokenText_NullEmptyOrWhitespace_Throws(string tokenText)
        {
            // Arrange
            var tokenDecoder = CreateTokenDecoder();

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                tokenDecoder.Decode(tokenText);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void TokenDecoder_Decode_ClaimDomains_Null_Works()
        {
            // Arrange
            var tokenDecoder = CreateTokenDecoder();
            string tokenText = "SomeFakeTokenText";
            string encodedTokenText = $"---{tokenText}---";
            IToken token = new Token
            {
                Text = tokenText
            };
            _MockJWTToken.Setup(m => m.DecodeToken(encodedTokenText))
                         .Returns(tokenText);
            List<ClaimDomain> claimDomains = null;
            _MockJWTToken.Setup(m => m.GetClaimDomains(tokenText))
                         .Returns(claimDomains);

            // Act
            var result = tokenDecoder.Decode(encodedTokenText);

            // Assert
            Assert.AreEqual(tokenText, token.Text);
            Assert.AreEqual(0, token.ClaimDomains.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void TokenDecoder_Decode_Null_ClaimDomains_Valid_Works()
        {
            // Arrange
            var tokenDecoder = CreateTokenDecoder();
            string tokenText = "SomeFakeTokenText";
            string encodedTokenText = $"---{tokenText}---";
            IToken token = new Token
            {
                Text = tokenText
            };
            _MockJWTToken.Setup(m => m.DecodeToken(encodedTokenText))
                         .Returns(tokenText);
            List<ClaimDomain> claimDomains = null;
            _MockJWTToken.Setup(m => m.GetClaimDomains(tokenText))
                         .Returns(claimDomains);

            // Act
            var result = tokenDecoder.Decode(encodedTokenText);

            // Assert
            Assert.AreEqual(tokenText, token.Text);
            Assert.AreEqual(0, token.ClaimDomains.Count);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void TokenDecoder_Decode_MultiUnitWithConcreteJWTToken_Test()
        {
            // Arrange
            var mockTokenKeyPair = _MockRepository.Create<ITokenKeyPair>();
            var publicKey = "-----BEGIN PUBLIC KEY-----" + Environment.NewLine +
                            "MFwwDQYJKoZIhvcNAQEBBQADSwAwSAJBAKPWgls8vUeDn/eXhPFWw4ZpHYu1AlKF" + Environment.NewLine +
                            "hw+t53LJe6w7HWlz7k1HES0I1WhbJnyzieCMn08XYbV6DCzgrygwnNUCAwEAAQ==" + Environment.NewLine +
                            "-----END PUBLIC KEY-----";
            mockTokenKeyPair.Setup(m => m.PublicKey).Returns(publicKey);
            var jwtToken = new JWTToken(mockTokenKeyPair.Object);
            var tokenDecoder = new TokenDecoder(jwtToken, _MockUserRoleEntityDataCache.Object);
            string tokenText = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.W3siU3ViamVjdCI6IlVzZXIiLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkiLCJDbGFpbXMiOlt7Ik5hbWUiOiJVc2VybmFtZSIsIlZhbHVlIjoid2FyZWhvdXNlb25lIiwiU3ViamVjdCI6IlVzZXIiLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkifSx7Ik5hbWUiOiJJZCIsIlZhbHVlIjoiNzIyNCIsIlN1YmplY3QiOiJVc2VyIiwiSXNzdWVyIjoiTE9DQUwgQVVUSE9SSVRZIn0seyJOYW1lIjoiTGFzdEF1dGhlbnRpY2F0ZWQiLCJWYWx1ZSI6IlR1ZSwgMjEgSmFuIDIwMjAgMDg6MzI6MTUgR01UIiwiU3ViamVjdCI6IlVzZXIiLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkifV19LHsiU3ViamVjdCI6Ik9yZ2FuaXphdGlvbiIsIklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSIsIkNsYWltcyI6W3siTmFtZSI6IklkIiwiVmFsdWUiOiIyMDMzMzgiLCJTdWJqZWN0IjoiT3JnYW5pemF0aW9uIiwiSXNzdWVyIjoiTE9DQUwgQVVUSE9SSVRZIn0seyJOYW1lIjoiTmFtZSIsIlZhbHVlIjoiV2FyZWhvdXNlIE9uZSIsIlN1YmplY3QiOiJPcmdhbml6YXRpb24iLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkifSx7Ik5hbWUiOiJTYXBJZCIsIlZhbHVlIjoiV2FyZWhvdXNlT25lIiwiU3ViamVjdCI6Ik9yZ2FuaXphdGlvbiIsIklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSJ9XX0seyJTdWJqZWN0IjoiVXNlclJvbGUiLCJJc3N1ZXIiOiJMT0NBTCBBVVRIT1JJVFkiLCJDbGFpbXMiOlt7Ik5hbWUiOiJSb2xlIiwiVmFsdWUiOiJDdXN0b21lciIsIlN1YmplY3QiOiJVc2VyUm9sZSIsIklzc3VlciI6IkxPQ0FMIEFVVEhPUklUWSJ9XX1d.LR9UoFZbmsyLdHiGDG74dVAzlLWLoDb_KiW3eLIr9kyFNTaSQp8tiSpwo10Gun3B5V1FS35J_jT_ykFY87SF0g";

            var role = "Customer";
            var roleId = 2;
            var userRoleIdMap = new Dictionary<string, int> { { role, roleId } };
            _MockUserRoleEntityDataCache.Setup(m => m.UserRoleIds).Returns(userRoleIdMap);

            // Act
            var result = tokenDecoder.Decode(tokenText);

            // Assert
            Assert.AreEqual(tokenText, result.Text);
            Assert.AreEqual(7224, result.CredentialEntityId);
            Assert.AreEqual(role, result.Role);
            Assert.AreEqual(roleId, result.RoleId);
        }
        #endregion
    }
}
