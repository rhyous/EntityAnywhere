using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices.Tests.WebServices
{
    [TestClass]
    public class AuthenticationWebServiceTests
    {
        private MockRepository _MockRepository;

        private Mock<IAuthenticationService> _MockAuthenticationService;
        private Mock<IRequestSourceIpAddress> _MockRequestSourceIpAddress;
        private Mock<ILogger> _MockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAuthenticationService = _MockRepository.Create<IAuthenticationService>();
            _MockRequestSourceIpAddress = _MockRepository.Create<IRequestSourceIpAddress>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private AuthenticationWebService CreateWebService()
        {
            return new AuthenticationWebService(
                _MockAuthenticationService.Object,
                _MockRequestSourceIpAddress.Object,
                _MockLogger.Object);
        }

        #region Authenticate
        [TestMethod]
        public async Task Service_AuthenticateAsync_CredentialsForwardedToService_Test()
        {
            // Arrange
            var service = CreateWebService();
            Credentials creds = new Credentials { User = "user27", Password = "passwd27!", AuthenticationPlugin = "Users"};
            var ipAddress = "10.27.0.1";
            _MockRequestSourceIpAddress.Setup(m => m.IpAddress).Returns(ipAddress);
            var token = new Token();
            _MockAuthenticationService.Setup(m => m.AuthenticateAsync(creds, ipAddress))
                                      .ReturnsAsync(token);

            // Act
            var result = await service.AuthenticateAsync(creds);

            // Assert
            Assert.AreEqual(token, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AuthenticateInQuery
        [TestMethod]
        public async Task Service_AuthenticateInQuery_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateWebService();
            string user = "user27";
            string password = "passwd27!";
            var ipAddress = "10.27.0.1";
            _MockRequestSourceIpAddress.Setup(m => m.IpAddress).Returns(ipAddress);
            var token = new Token();
            _MockAuthenticationService.Setup(m => m.AuthenticateAsync(It.Is<Credentials>(c=>c.User == user && c.Password == password), ipAddress))
                                      .ReturnsAsync(token);

            // Act
            var result = await service.AuthenticateInQueryAsync(user,password);

            // Assert
            Assert.AreEqual(token, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Dispose
        [TestMethod]
        public void Service_Dispose_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateWebService();
            _MockAuthenticationService.Setup(m => m.Dispose());

            // Act
            service.Dispose();

            // Assert
            Assert.IsTrue(service._IsDisposed);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
