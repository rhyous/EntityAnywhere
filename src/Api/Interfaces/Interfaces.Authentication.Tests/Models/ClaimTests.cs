using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Models
{
    [TestClass]
    public class ClaimTests
    {
        #region Subject
        [TestMethod]
        public void Claim_Subject_ComesFromParent()
        {
            // Arrange
            var parent = new ClaimDomain { Subject = "Subject27" };
            var claim = new Claim();
            parent.Claims.Add(claim);

            // Act
            var actual = claim.Subject;

            // Assert
            Assert.AreEqual(parent.Subject, actual);
        }

        [TestMethod]
        public void Claim_Subject_ComesFromParent_ParentNull()
        {
            // Arrange
            var claim = new Claim();

            // Act
            var actual = claim.Subject;

            // Assert
            Assert.IsNull(actual);
        }
        #endregion

        #region Issuer
        [TestMethod]
        public void Claim_Issuer_ComesFromParent()
        {
            // Arrange
            var parent = new ClaimDomain { Issuer = "Issuer27" };
            var claim = new Claim();
            parent.Claims.Add(claim);

            // Act
            var actual = claim.Issuer;

            // Assert
            Assert.AreEqual(parent.Issuer, actual);
        }

        [TestMethod]
        public void Claim_Issuer_ComesFromParent_ParentNull()
        {
            // Arrange
            var claim = new Claim();

            // Act
            var actual = claim.Issuer;

            // Assert
            Assert.IsNull(actual);
        }
        #endregion
    }
}