using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests.Business
{
    [TestClass]
    public class HeadersUpdaterTests
    {
        #region Update
        [TestMethod]
        public void HeadersUpdater_Update_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var headersUpdater = new HeadersUpdater();
            var token = new AccessToken { UserId = 2219 };
            var headers = new NameValueCollection();

            // Act
            headersUpdater.Update(token, headers);

            // Assert - Nothing
        }

        [TestMethod]
        public void HeadersUpdater_Update_ValidClaimValues_Test()
        {
            // Arrange
            var headersUpdater = new HeadersUpdater();
            var token = new AccessToken {  UserId = 9966301, Subject = "user9966301" };
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
