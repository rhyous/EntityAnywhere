using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Exceptions;

namespace Rhyous.EntityAnywhere.Clients2.Tests.Models
{
    [TestClass]
    public class AuthenticationSettingsTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientConfig> _MockEntityClientConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConfig = _MockRepository.Create<IEntityClientConfig>();
        }

        private AuthenticationSettings CreateAuthenticationSettings()
        {
            return new AuthenticationSettings(_MockEntityClientConfig.Object);
        }

        #region ServiceUrl
        [TestMethod]
        public void AuthenticationSettings_ServiceUrl_WithSubPath_Tests()
        {
            // Arrange
            var authenticationSettings = CreateAuthenticationSettings();
            var host = "https://somedomain.tld";
            var subPath = "Api";
            var service = "AuthenticationService";
            var expected = $"{host}/{subPath}/{service}";

            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);
            _MockEntityClientConfig.Setup(m => m.EntitySubpath).Returns(subPath);

            // Act
            var result = authenticationSettings.ServiceUrl;

            // Assert
            Assert.AreEqual(expected, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void AuthenticationSettings_ServiceUrl_WithOutSubPath_Tests()
        {
            // Arrange
            var authenticationSettings = CreateAuthenticationSettings();
            var host = "https://somedomain.tld";
            var subPath = "";
            var service = "AuthenticationService";
            var expected = $"{host}/{service}";

            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);
            _MockEntityClientConfig.Setup(m => m.EntitySubpath).Returns(subPath);

            // Act
            var result = authenticationSettings.ServiceUrl;

            // Assert
            Assert.AreEqual(expected, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void AuthenticationSettings_ServiceUrl_EntityWebHost_NotConfigured_Throws(string host)
        {
            // Arrange
            var authenticationSettings = CreateAuthenticationSettings();
            var subPath = "Api";
            var service = "AuthenticationService";
            var expected = $"{host}/{subPath}/{service}";

            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);

            // Act
            // Assert
            Assert.ThrowsException<MissingConfigurationException>(() =>
            {
                var result = authenticationSettings.ServiceUrl;
            });
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}