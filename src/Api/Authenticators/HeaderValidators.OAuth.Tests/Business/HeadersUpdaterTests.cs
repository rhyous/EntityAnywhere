using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
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
            var headers = new Mock<IHeadersContainer>();

            // Act
            headersUpdater.Update(token, headers.Object);

            // Assert
            headers.VerifyAll();
        }

        [TestMethod]
        public void HeadersUpdater_Update_ValidClaimValues_Test()
        {
            // Arrange
            var headersUpdater = new HeadersUpdater();
            var token = new AccessToken {  UserId = 9966301, Subject = "user9966301" };
            var headers = new Mock<IHeadersContainer>();

            // Act
            headersUpdater.Update(token, headers.Object);

            // Assert
            headers.VerifyAll();
        }
        #endregion
    }
}
