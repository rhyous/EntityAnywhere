using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class HeadersUpdaterTests
    {
        private MockRepository _MockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
        }

        #region Update

        [TestMethod]
        public void HeadersUpdater_Update_ValidClaimValues_Test()
        {
            // Arrange
            var headersUpdater = new HeadersUpdater();
            var orgIdClaim = new Claim { Subject = "Organization", Name = "Id", Value = "1027" };
            var orgClaimDomain = new ClaimDomain { Subject = "Organization"};
            orgClaimDomain.Claims.Add(orgIdClaim);

            var userIdClaim = new Claim { Subject = "User", Name = "Id", Value = "9966301" };
            var userNameClaim = new Claim { Subject = "User", Name = "Username", Value = "user9966301" };
            var userClaimDomain = new ClaimDomain { Subject = "User" };
            userClaimDomain.Claims.Add(userIdClaim);
            userClaimDomain.Claims.Add(userNameClaim);

            var token = new Token { ClaimDomains = new List<ClaimDomain> { orgClaimDomain, userClaimDomain } };

            var mockHeaders = _MockRepository.Create<IHeadersContainer>();
            mockHeaders.Setup(m => m.Get("UserId")).Returns("9966301");
            mockHeaders.Setup(m => m.Remove("UserId"));
            mockHeaders.Setup(m => m.Add("UserId", "9966301"));
            mockHeaders.Setup(m => m.Get("Username")).Returns("user9966301");
            mockHeaders.Setup(m => m.Remove("Username"));
            mockHeaders.Setup(m => m.Add("Username", "user9966301"));

            // Act
            headersUpdater.Update(token, mockHeaders.Object);

            // Assert
            _MockRepository.VerifyAll();

        }
        #endregion
    }
}