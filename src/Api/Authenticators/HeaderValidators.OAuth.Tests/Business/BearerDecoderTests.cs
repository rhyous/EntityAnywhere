using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class BearerDecoderTests
    {
        private MockRepository _MockRepository;

        private Mock<IJWTValidator> _MockJWTValidator;
        private Mock<ITokenFromClaimsBuilder> _MockTokenFromClaimsBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockJWTValidator = _MockRepository.Create<IJWTValidator>();
            _MockTokenFromClaimsBuilder = _MockRepository.Create<ITokenFromClaimsBuilder>();
        }

        private BearerDecoder CreateBearerDecoder()
        {
            return new BearerDecoder(
                _MockJWTValidator.Object,
                _MockTokenFromClaimsBuilder.Object);
        }

        #region DecodeAsync
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task BearerDecoder_DecodeAsync_tokenText_NullEmptyOrWhitespace_Throws(string tokenText)
        {
            // Arrange
            var bearerDecoder = CreateBearerDecoder();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await bearerDecoder.DecodeAsync(tokenText);
            }, $"'{nameof(tokenText)}' cannot be null or whitespace.");
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task BearerDecoder_DecodeAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var bearerDecoder = CreateBearerDecoder();
            string tokenText = "someToken";

            var claims = new ClaimsPrincipal();
            _MockJWTValidator.Setup(m => m.ValidateAsync(tokenText))
                             .ReturnsAsync(claims);

            var token = new AccessToken();
            _MockTokenFromClaimsBuilder.Setup(m => m.Build(claims))
                                       .Returns(token);

            // Act
            var result = await bearerDecoder.DecodeAsync(tokenText);

            // Assert
            Assert.AreEqual(token, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
