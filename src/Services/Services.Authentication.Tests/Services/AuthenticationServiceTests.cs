using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private MockRepository _MockRepository;

        private Mock<ICredentialsValidatorAsync> _MockCredentialsValidator;
        private Mock<IAccountLocker> _MockAccountLocker;
        private Mock<IBasicAuth> _MockBasicAuth;
        private Mock<ILogger> _MockLogger;


        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockCredentialsValidator = _MockRepository.Create<ICredentialsValidatorAsync>();
            _MockAccountLocker = _MockRepository.Create<IAccountLocker>();
            _MockBasicAuth = _MockRepository.Create<IBasicAuth>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private AuthenticationService CreateService()
        {
            return new AuthenticationService(
                _MockCredentialsValidator.Object,
                _MockAccountLocker.Object,
                _MockBasicAuth.Object,
                _MockLogger.Object);
        }

        [TestMethod]
        public async Task AuthenticationService_TooManyLoginAttemptsThrowsException()
        {
            // Arrange
            var creds = new Credentials { User = "user1", Password = "pw-1234" };
            var service = CreateService();

            AuthenticationAttempt actualAA = null;

            _MockAccountLocker.Setup(m => m.IsLocked(creds)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
            {
                await service.AuthenticateAsync(creds, "::1");
            });
            Assert.IsNull(actualAA);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthenticationService_Failed_LoginAttempt_NullResponse()
        {
            // Arrange
            var creds = new Credentials { User = "user1", Password = "pw-1234" };
            var service = CreateService();

            AuthenticationAttempt actualAA = null;

            _MockAccountLocker.Setup(m => m.IsLocked(creds)).ReturnsAsync(false);
            _MockAccountLocker.Setup(m => m.AddAttempt(It.IsAny<AuthenticationAttempt>()))
                              .Returns((AuthenticationAttempt attempt) =>
                              {
                                  actualAA = attempt;
                                  return Task.CompletedTask;
                              });

            _MockCredentialsValidator.Setup(m => m.IsValidAsync(creds))
                                     .ReturnsAsync((CredentialsValidatorResponse)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
            {
                await service.AuthenticateAsync(creds, "::1");
            });
            Assert.AreEqual("Failure", actualAA.Result);

            Assert.AreEqual("::1", actualAA.IpAddress);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthenticationService_Failed_LoginAttempt_FailedResponse()
        {
            // Arrange
            var creds = new Credentials { User = "user1", Password = "pw-1234" };
            var service = CreateService();

            AuthenticationAttempt actualAA = null;

            _MockAccountLocker.Setup(m => m.IsLocked(creds)).ReturnsAsync(false);
            _MockAccountLocker.Setup(m => m.AddAttempt(It.IsAny<AuthenticationAttempt>()))
                              .Returns((AuthenticationAttempt attempt) =>
                              {
                                  actualAA = attempt;
                                  return Task.CompletedTask;
                              });

            var reponse = new CredentialsValidatorResponse
            {
                AuthenticationPlugin = "Plugin1",
                Token = Data.Token,
                Success = false,
                Message = "Failed"
            };
            _MockCredentialsValidator.Setup(m => m.IsValidAsync(creds))
                                     .ReturnsAsync(reponse);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AuthenticationException>(async () =>
            {
                await service.AuthenticateAsync(creds, "::1");
            });
            Assert.AreEqual("Failure", actualAA.Result);

            Assert.AreEqual("::1", actualAA.IpAddress);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task AuthenticationService_Unknown_LoginAttempt_ExceptionNotRelatedToAuthentication()
        {
            // Arrange
            var creds = new Credentials { User = "user1", Password = "pw-1234" };
            var service = CreateService();

            AuthenticationAttempt actualAA = null;

            _MockAccountLocker.Setup(m => m.IsLocked(creds)).ReturnsAsync(false);
            _MockAccountLocker.Setup(m => m.AddAttempt(It.IsAny<AuthenticationAttempt>()))
                             .Callback((AuthenticationAttempt attempt) => { actualAA = attempt; })
                             .Returns(Task.CompletedTask);

            _MockCredentialsValidator.Setup(m => m.IsValidAsync(creds)).ReturnsAsync((Credentials inCreds) => { throw new Exception(); });

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () =>
            {
                await service.AuthenticateAsync(creds, "::1");
            });
            Assert.AreEqual("Unknown", actualAA.Result);
            Assert.AreEqual("::1", actualAA.IpAddress);
            _MockRepository.VerifyAll();
        }
    }
}