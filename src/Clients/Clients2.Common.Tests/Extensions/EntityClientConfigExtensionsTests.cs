using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Exceptions;
using System;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.Extensions
{
    [TestClass]
    public class EntityClientConfigExtensionsTests
    {
        private MockRepository _MockRepository;
        private Mock<IEntityClientConfig> _MockEntityClientConfig;



        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockEntityClientConfig = _MockRepository.Create<IEntityClientConfig>();
        }

        #region GetServiceUrl
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void EntityClientConfigExtensions_GetServiceUrl_ServiceUrl_Null(string service)
        {
            // Arrange
            IEntityClientConfig entityClientConfig = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                entityClientConfig.GetServiceUrl(service);
            });
            _MockRepository.VerifyAll();
        }
        #endregion

        #region ServiceUrl
        [TestMethod]
        public void EntityClientConfigExtensions_ServiceUrl_WithSubPath_Tests()
        {
            // Arrange
            var host = "https://somedomain.tld";
            var subPath = "Api";
            var service = "AuthenticationService";
            var expected = $"{host}/{subPath}/{service}";

            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);
            _MockEntityClientConfig.Setup(m => m.EntitySubpath).Returns(subPath);

            // Act
            var result = _MockEntityClientConfig.Object.GetServiceUrl(service);

            // Assert
            Assert.AreEqual(expected, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void EntityClientConfigExtensions_ServiceUrl_WithOutSubPath_Tests()
        {
            // Arrange
            var host = "https://somedomain.tld";
            var subPath = "";
            var service = "AuthenticationService";
            var expected = $"{host}/{service}";

            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);
            _MockEntityClientConfig.Setup(m => m.EntitySubpath).Returns(subPath);

            // Act
            var result = _MockEntityClientConfig.Object.GetServiceUrl(service);

            // Assert
            Assert.AreEqual(expected, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void EntityClientConfigExtensions_ServiceUrl_EntityWebHost_NotConfigured_Throws(string host)
        {
            // Arrange
            var service = "AuthenticationService";
            _MockEntityClientConfig.Setup(m => m.EntityWebHost).Returns(host);

            // Act
            // Assert
            Assert.ThrowsException<MissingConfigurationException>(() =>
            {
                var result = _MockEntityClientConfig.Object.GetServiceUrl(service);
            });
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
