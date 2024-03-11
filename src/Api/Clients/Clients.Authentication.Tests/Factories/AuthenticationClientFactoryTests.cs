using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Collections;
using Rhyous.UnitTesting;

namespace Rhyous.EntityAnywhere.Clients2.Tests.Factories
{
    [TestClass]
    public class AuthenticationClientFactoryTests
    {
        private MockRepository _MockRepository;
        private Mock<IAuthenticationClient> _MockAuthenticationClient;

        private IContainer _Container;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAuthenticationClient = _MockRepository.Create<IAuthenticationClient>();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_MockAuthenticationClient.Object)
                   .As<IAuthenticationClient>();
            _Container = builder.Build();
        }

        private AuthenticationClientFactory CreateFactory()
        {
            return new AuthenticationClientFactory(_Container.BeginLifetimeScope());
        }

        #region CreateAuthenticationClient
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void AuthenticationClientFactory_CreateAuthenticationClient_ServiceUrl_NullEmptyOrWhitespace_UsesConfiguredHost(string serviceUrl)
        {
            // Arrange
            var factory = CreateFactory();

            // Act
            var result = factory.Create(serviceUrl);

            // Assert
            Assert.AreEqual(result, _MockAuthenticationClient.Object);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void AuthenticationClientFactory_CreateAuthenticationClient_ServiceUrl_CreatesClientWithServiceUrl()
        {
            // Arrange
            var factory = CreateFactory();
            var serviceUrl = "http://somedomain.tld";

            // Act
            var result = factory.Create(serviceUrl);

            // Assert
            Assert.AreNotEqual(result, _MockAuthenticationClient.Object);
            Assert.IsNotNull(result);
            var settings = (result.GetFieldValue("_AuthenticationSettings") as IAuthenticationSettings);
            Assert.AreEqual(serviceUrl, settings.ServiceUrl);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
