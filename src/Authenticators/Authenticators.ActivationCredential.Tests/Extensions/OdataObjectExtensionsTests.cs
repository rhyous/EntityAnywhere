using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata;
using Rhyous.WebFramework.Entities;

namespace Rhyous.WebFramework.Authenticators.Tests.Extensions
{
    [TestClass]
    public class OdataObjectExtensionsTests
    {
        #region GetSapId
        [TestMethod]
        public void OdataObjectExtensions_GetSapId_NoAlternateIdRelatedEntities_ReturnsOrgDotSapId()
        {
            // Arrange
            var org = new Organization { Id = 1027, Name = "Some Org 1027", SapId = "9900001027" };
            var odataOrg = org.AsOdata<Organization, int>();

            // Act
            var result = odataOrg.GetSapId();

            // Assert
            Assert.AreEqual(org.SapId, result);
        }

        [TestMethod]
        public void OdataObjectExtensions_GetSapId_AlternateIdRelatedEntitiesExist_ButNonForSapId_ReturnsOrgDotSapId()
        {
            // Arrange
            var org = new Organization { Id = 1027, Name = "Some Org 1027", SapId = "9900001027" };
            var odataOrg = org.AsOdata<Organization, int>();
            var altIdHda = new AlternateId { Id = 20, Entity = nameof(Organization), EntityId = org.Id.ToString(), Property = "Hda", Value = "1027" };
            var odataAltIdCollection = new[] { altIdHda }.AsOdata<AlternateId, long>();
            odataOrg.RelatedEntityCollection.Add(odataAltIdCollection);

            // Act
            var result = odataOrg.GetSapId();

            // Assert
            Assert.AreEqual(org.SapId, result);
        }

        [TestMethod]
        public void OdataObjectExtensions_GetSapId_AlternateId_SapId_Exists_ReturnsIt()
        {
            // Arrange
            var org = new Organization { Id = 1027, Name = "Some Org 1027", SapId = "9900001027" };
            var odataOrg = org.AsOdata<Organization, int>();
            var altIdHda = new AlternateId { Id = 20, Entity = nameof(Organization), EntityId = org.Id.ToString(), Property = "Hda", Value = "1027" };
            // the -x in 990001027-x would exist in real runtime, but for this test, it differentiates it from org.SapId
            var altIdSapId = new AlternateId { Id = 20, Entity = nameof(Organization), EntityId = org.Id.ToString(), Property = "SapId", Value = "990001027-x" }; 
            var odataAltIdCollection = new[] { altIdHda, altIdSapId }.AsOdata<AlternateId, long>();
            odataOrg.RelatedEntityCollection.Add(odataAltIdCollection);

            // Act
            var result = odataOrg.GetSapId();

            // Assert
            Assert.AreEqual(altIdSapId.Value, result);
        }

        [TestMethod]
        public void OdataObjectExtensions_GetSapId_AlternateId_SapId_Missing_ButS4Id_Exists_ReturnsIt()
        {
            // Arrange
            var org = new Organization { Id = 1027, Name = "Some Org 1027", SapId = "9900001027" };
            var odataOrg = org.AsOdata<Organization, int>();
            var altIdHda = new AlternateId { Id = 20, Entity = nameof(Organization), EntityId = org.Id.ToString(), Property = "Hda", Value = "1027" };
            // the -x in 990001027-x would exist in real runtime, but for this test, it differentiates it from org.SapId
            var altIdS4Id = new AlternateId { Id = 20, Entity = nameof(Organization), EntityId = org.Id.ToString(), Property = "S4Id", Value = "990001027-x" };
            var odataAltIdCollection = new[] { altIdHda, altIdS4Id }.AsOdata<AlternateId, long>();
            odataOrg.RelatedEntityCollection.Add(odataAltIdCollection);

            // Act
            var result = odataOrg.GetSapId();

            // Assert
            Assert.AreEqual(altIdS4Id.Value, result);
        }
        #endregion
    }
}
