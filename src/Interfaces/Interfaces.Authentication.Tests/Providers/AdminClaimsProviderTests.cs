using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Providers
{
    [TestClass]
    public class AdminClaimsProviderTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        private AdminClaimsProvider CreateProvider()
        {
            return new AdminClaimsProvider();
        }

        #region ClaimDomains
        [TestMethod]
        public void AdminCliamsProvider_ClaimDomains_Test()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var actual = provider.ClaimDomains;

            // Assert
            Assert.AreEqual(3, actual.Count);
        }

        [TestMethod]
        public void AdminCliamsProvider_ClaimDomains_NewClaimsCreatedEveryCall_Test()
        {
            // Arrange
            var provider = CreateProvider();

            // Act
            var actual1 = provider.ClaimDomains;
            var actual2 = provider.ClaimDomains;

            // Assert
            Assert.AreNotEqual(actual1, actual2);
        }
        #endregion
    }
}