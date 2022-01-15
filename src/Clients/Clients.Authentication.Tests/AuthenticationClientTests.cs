using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2.Tests
{
    [TestClass]
    public class AuthenticationClientTests
    {
        private MockRepository _MockRepository;

        private Mock<IAuthenticationSettings> _MockAuthenticationSettings;
        private Mock<IHttpClientRunner> _MockHttpClientRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAuthenticationSettings = _MockRepository.Create<IAuthenticationSettings>();
            _MockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
        }

        private AuthenticationClient CreateAuthenticationClient()
        {
            return new AuthenticationClient(
                _MockAuthenticationSettings.Object,
                _MockHttpClientRunner.Object);
        }

        #region Authenticate user password
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task AuthenticationClient_Authenticate_User_Null_Throws(string user)
        {
            // Arrange
            var authenticationClient = CreateAuthenticationClient();
            string password = "somepasswd";

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await authenticationClient.AuthenticateAsync(user, password);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task AuthenticationClient_Authenticate_Password_Null_Throws(string password)
        {
            // Arrange
            var authenticationClient = CreateAuthenticationClient();
            string user = "user101";

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await authenticationClient.AuthenticateAsync(user, password);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthenticationClient_Authenticate_Works()
        {
            // Arrange
            var authenticationClient = CreateAuthenticationClient();
            string user = "user101";
            string password = "somepasswd";

            var serviceUrl = "https://somedomain.tld/Api";
            _MockAuthenticationSettings.Setup(m => m.ServiceUrl).Returns(serviceUrl);

            var action = "Authenticate";
            _MockAuthenticationSettings.Setup(m => m.Action).Returns(action);

            var actionUrl = $"{serviceUrl}/{action}";
            var token = new Token();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<Token>(HttpMethod.Post, actionUrl, It.IsAny<StringContent>(), true))
                                 .ReturnsAsync(token);

            // Act
            var result = await authenticationClient.AuthenticateAsync(user, password);

            // Assert
            Assert.AreEqual(token, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Authenticate Credentials
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task AuthenticationClient_Authenticate_Credentials_User_Null_Throws(string user)
        {
            // Arrange
            var authenticationClient = CreateAuthenticationClient();
            string password = "somepasswd";
            var creds = new Credentials { User = user, Password = password };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await authenticationClient.AuthenticateAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task AuthenticationClient_Authenticate_Credentials_Password_Null_Throws(string password)
        {
            // Arrange
            var authenticationClient = CreateAuthenticationClient();
            string user = "user101";
            var creds = new Credentials { User = user, Password = password };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await authenticationClient.AuthenticateAsync(creds);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthenticationClient_Authenticate_Credentials_Works()
        {
            // Arrange
            var authenticationClient = CreateAuthenticationClient();
            string user = "user101";
            string password = "somepasswd";
            var creds = new Credentials { User = user, Password = password };

            var serviceUrl = "https://somedomain.tld/Api";
            _MockAuthenticationSettings.Setup(m => m.ServiceUrl).Returns(serviceUrl);

            var action = "Authenticate";
            _MockAuthenticationSettings.Setup(m => m.Action).Returns(action);

            var actionUrl = $"{serviceUrl}/{action}";
            var token = new Token();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<Token>(HttpMethod.Post, actionUrl, It.IsAny<StringContent>(), true))
                                 .ReturnsAsync(token);

            // Act
            var result = await authenticationClient.AuthenticateAsync(creds);

            // Assert
            Assert.AreEqual(token, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}