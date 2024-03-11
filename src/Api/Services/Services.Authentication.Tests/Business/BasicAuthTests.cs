using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class BasicAuthTests
    {
        private MockRepository _MockRepository;

        private Mock<IHeaders> _MockHeaders;
        private Mock<IBasicAuthEncoder> _MockBasicAuthEncoder;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockHeaders = _MockRepository.Create<IHeaders>();
            _MockBasicAuthEncoder = _MockRepository.Create<IBasicAuthEncoder>();
        }

        private BasicAuth CreateBasicAuth()
        {
            return new BasicAuth(
                _MockHeaders.Object,
                _MockBasicAuthEncoder.Object);
        }

        #region BasicAuth.Credentials
        [TestMethod]
        public void BasicAuth_Credentials_HeadersCollection_Null()
        {
            // Arrange
            var basicAuth = CreateBasicAuth();
            _MockHeaders.Setup(m => m.Collection).Returns((NameValueCollection)null);

            // Act
            // Assert
            Assert.IsNull(basicAuth.Credentials);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BasicAuth_Credentials_HeadersCollection_Empty()
        {
            // Arrange
            var basicAuth = CreateBasicAuth();
            var nvc = new NameValueCollection();
            _MockHeaders.Setup(m => m.Collection).Returns(nvc);

            // Act
            // Assert
            Assert.IsNull(basicAuth.Credentials);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void BasicAuth_Credentials_HeadersCollection_Valid()
        {
            // Arrange
            var basicAuth = CreateBasicAuth();
            var nvc = new NameValueCollection { { "Authorization", ""} };
            _MockHeaders.Setup(m => m.Collection).Returns(nvc);

            // Act
            // Assert
            Assert.IsNull(basicAuth.Credentials);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
