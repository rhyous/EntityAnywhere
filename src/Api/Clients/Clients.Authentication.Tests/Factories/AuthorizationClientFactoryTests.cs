using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;

namespace Rhyous.EntityAnywhere.Clients2.Tests.Factories
{
    [TestClass]
    public class AuthorizationClientFactoryTests
    {
        private MockRepository _MockRepository;

        private Mock<ILifetimeScope> _MockLifetimeScope;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockLifetimeScope = _MockRepository.Create<ILifetimeScope>();
        }

        private AuthorizationClientFactory CreateFactory()
        {
            return new AuthorizationClientFactory(
                _MockLifetimeScope.Object);
        }

        #region Create
        [TestMethod]
        public void AuthorizationClientFactory_Create_WithOptionalParams_Test()
        {
            // Arrange
            var factory = CreateFactory();
            string token = "some fake token";
            string serviceUrl = "https://some.site.tld/subpath/AuthorizationService";

            // Act
            var result = factory.Create(token, serviceUrl);

            // Assert
            Assert.AreEqual(serviceUrl, result.GetFieldValue("_ServiceUrl"));
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
