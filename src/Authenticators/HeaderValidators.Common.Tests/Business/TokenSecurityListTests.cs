using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.HeaderValidators.Tests
{
    [TestClass]
    public class TokenSecurityListTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        #region GetCustomerCalls
        [TestMethod]
        public void TokenSecurityList_GetCustomerCalls()
        {
            // Arrange
            // Act
            var result = new TokenSecurityList().GetCustomerCalls();

            // Assert
            Assert.IsTrue(result.Count > 0);
        }

        #endregion
    }
}
