using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class AccountLockerTests
    {
        [TestMethod]
        public async Task AcountLocker_IsLocked_Test()
        {
            // Arrange
            var authenticationAttempt1 = new AuthenticationAttempt { Id = 1, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-59), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var authenticationAttempt2 = new AuthenticationAttempt { Id = 2, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-50), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var authenticationAttempt3 = new AuthenticationAttempt { Id = 3, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-30), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var attempts = new List<AuthenticationAttempt> { authenticationAttempt1, authenticationAttempt2, authenticationAttempt3 };
            var odataAttempts = attempts.AsOdata<AuthenticationAttempt, long>();
            var mockSettings = new Mock<IAuthenticationSettings>();
            var dateTimeOffsetzzz = DateTimeOffset.Now.ToString("zzz");
            var dateTimeOffsetzzzEscaped = Uri.EscapeDataString(dateTimeOffsetzzz);
            mockSettings.Setup(m => m.Start).Returns(DateTimeOffset.Parse($"1/8/2020 2:42:38 PM {dateTimeOffsetzzz}", CultureInfo.InvariantCulture));
            var mockAuthenticationAttemptClient = new Mock<IAdminEntityClientAsync<AuthenticationAttempt, long>>();
            string actualQueryParams = "";
            var expectedQueryParams = $"$filter=Username eq 'user1' and Result eq Failure and Ignore eq 0 and CreateDate gt \"1%2F8%2F2020%202%3A42%3A38%20PM%20{dateTimeOffsetzzzEscaped}\"";
            mockAuthenticationAttemptClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                           .ReturnsAsync((string queryParams, bool inBool) =>
                                           {
                                               actualQueryParams = queryParams;
                                               return odataAttempts;
                                           });

            var locker = new AccountLocker(mockAuthenticationAttemptClient.Object, mockSettings.Object);

            var creds = new Credentials { User = "user1", Password = "" };

            // Act 
            var actual = await locker.IsLocked(creds);

            // Assert
            Assert.IsTrue(actual);
            Assert.AreEqual(expectedQueryParams, actualQueryParams);
            mockAuthenticationAttemptClient.Verify(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public async Task AcountLocker_IsLocked_MaxFailedAttempts_ChangedTo4_Test()
        {
            // Arrange
            var authenticationAttempt1 = new AuthenticationAttempt { Id = 1, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-59), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var authenticationAttempt2 = new AuthenticationAttempt { Id = 2, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-50), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var authenticationAttempt3 = new AuthenticationAttempt { Id = 3, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-30), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var attempts = new List<AuthenticationAttempt> { authenticationAttempt1, authenticationAttempt2, authenticationAttempt3 };
            var odataAttempts = attempts.AsOdata<AuthenticationAttempt, long>();

            var mockAuthenticationAttemptClient = new Mock<IAdminEntityClientAsync<AuthenticationAttempt, long>>();
            mockAuthenticationAttemptClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                           .ReturnsAsync(odataAttempts);

            var creds = new Credentials { User = "user1", Password = "" };
            
            var mockSettings = new Mock<IAuthenticationSettings>();
            mockSettings.SetupGet(m => m.MaxFailedAttempts).Returns(4);

            var locker = new AccountLocker(mockAuthenticationAttemptClient.Object, mockSettings.Object);

            // Act
            var actual = await locker.IsLocked(creds);

            // Assert
            Assert.IsFalse(actual);
            mockAuthenticationAttemptClient.Verify(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public async Task AcountLocker_IsLocked_MaxFailedAttemptsMinutes_ChangedTo50_Test()
        {
            // Arrange
            var authenticationAttempt1 = new AuthenticationAttempt { Id = 1, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-59), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var authenticationAttempt2 = new AuthenticationAttempt { Id = 2, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-50), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var authenticationAttempt3 = new AuthenticationAttempt { Id = 3, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-30), CreatedBy = 1, IpAddress = "::1", Ignore = false };
            var attempts = new List<AuthenticationAttempt> { authenticationAttempt1, authenticationAttempt2, authenticationAttempt3 };
            var odataAttempts = attempts.AsOdata<AuthenticationAttempt, long>();

            var mockAuthenticationAttemptClient = new Mock<IAdminEntityClientAsync<AuthenticationAttempt, long>>();
            mockAuthenticationAttemptClient.Setup(m => m.GetByQueryParametersAsync(It.IsAny<string>(), It.IsAny<bool>()))
                                           .ReturnsAsync(odataAttempts);
            AuthenticationAttempt actualAA = null;
            mockAuthenticationAttemptClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<AuthenticationAttempt>>(), false))
                                           .Callback((IEnumerable<AuthenticationAttempt> posted, bool b) =>
                                           {
                                               actualAA = posted.First();
                                           })
                                           .ReturnsAsync(new[] { actualAA }.AsOdata<AuthenticationAttempt, long>());

            var creds = new Credentials { User = "user1", Password = "" };

            var mockSettings = new Mock<IAuthenticationSettings>();
            mockSettings.SetupGet(m => m.MaxFailedAttemptsMinutes).Returns(50);

            var locker = new AccountLocker(mockAuthenticationAttemptClient.Object, mockSettings.Object);

            // Act
            var actual = await locker.IsLocked(creds);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public async Task AcountLocker_AddAttempt_Test()
        {
            // Arrange
            var authenticationAttempt = new AuthenticationAttempt { Id = 1, Username = "user1", Result = "Failure", CreateDate = DateTimeOffset.Now.AddMinutes(-59), CreatedBy = 1, IpAddress = "::1", Ignore = false };

            AuthenticationAttempt actualAA = null;
            var mockAuthenticationAttemptClient = new Mock<IAdminEntityClientAsync<AuthenticationAttempt, long>>();
            mockAuthenticationAttemptClient.Setup(m => m.PostAsync(It.IsAny<IEnumerable<AuthenticationAttempt>>(), false))
                                           .Callback((IEnumerable<AuthenticationAttempt> posted, bool b) =>
                                           {
                                               actualAA = posted.First();
                                           })
                                           .ReturnsAsync(new[] { actualAA }.AsOdata<AuthenticationAttempt, long>());
            var creds = new Credentials { User = "user1", Password = "" };
            
            var mockSettings = new Mock<IAuthenticationSettings>();
            mockSettings.SetupGet(m => m.MaxFailedAttemptsMinutes).Returns(50);

            var locker = new AccountLocker(mockAuthenticationAttemptClient.Object, mockSettings.Object);

            // Act
            await locker.AddAttempt(authenticationAttempt);

            // Assert
            mockAuthenticationAttemptClient.Verify(m => m.PostAsync(It.IsAny<IEnumerable<AuthenticationAttempt>>(), false), Times.Once);
            Assert.AreEqual(authenticationAttempt, actualAA);
        }
    }
}