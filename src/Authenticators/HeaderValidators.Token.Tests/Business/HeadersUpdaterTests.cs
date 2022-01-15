using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class HeadersUpdaterTests
    {
        #region Update
        [TestMethod]
        public void HeadersUpdater_Update_NoErrorsIfEmpty_Test()
        {
            // Arrange
            var headersUpdater = new HeadersUpdater();
            var token = new Token();
            var headers = new NameValueCollection();

            // Act
            headersUpdater.Update(token, headers);

            // Assert - nothing
        }

        [TestMethod]
        public void HeadersUpdater_Update_ValidClaimValues_Test()
        {
            // Arrange
            var headersUpdater = new HeadersUpdater();
            var orgIdClaim = new Claim { Subject = "Organization", Name = "Id", Value = "1027" };
            var orgSapIdClaim = new Claim { Subject = "Organization", Name = "SapId", Value = "9900001027" };
            var orgClaimDomain = new ClaimDomain { Subject = "Organization"};
            orgClaimDomain.Claims.Add(orgIdClaim);
            orgClaimDomain.Claims.Add(orgSapIdClaim);

            var userIdClaim = new Claim { Subject = "User", Name = "Id", Value = "9966301" };
            var userNameClaim = new Claim { Subject = "User", Name = "Username", Value = "user9966301" };
            var userClaimDomain = new ClaimDomain { Subject = "User" };
            userClaimDomain.Claims.Add(userIdClaim);
            userClaimDomain.Claims.Add(userNameClaim);

            var token = new Token { ClaimDomains = new List<ClaimDomain> { orgClaimDomain, userClaimDomain } };
            var headers = new NameValueCollection();

            // Act
            headersUpdater.Update(token, headers);

            // Assert
            Assert.AreEqual("1027", headers.Get("OrganizationId"));
            Assert.AreEqual("9900001027", headers.Get("SapId"));
            Assert.AreEqual("9966301", headers.Get("UserId"));
            Assert.AreEqual("user9966301", headers.Get("Username"));

        }
        #endregion
    }
}