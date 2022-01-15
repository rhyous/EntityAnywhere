using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Models
{
    [TestClass]
    public class ClaimDomainTests
    {
        private ClaimDomain CreateClaimDomain()
        {
            return new ClaimDomain();
        }

        #region Claims
        [TestMethod]
        public void ClaimDomain_Claims_LazyInstantiated()
        {
            // Arrange
            var claimDomain = this.CreateClaimDomain();

            // Act
            var actual = claimDomain.Claims;

            // Assert
            Assert.IsNotNull(actual);
        }
        #endregion
    }
}