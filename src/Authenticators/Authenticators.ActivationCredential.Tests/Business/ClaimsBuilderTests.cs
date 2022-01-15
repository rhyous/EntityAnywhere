using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.UnitTesting;
using Rhyous.WebFramework.Clients2;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Exceptions;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Authenticators.Tests
{
    [TestClass]
    public class ClaimsBuilderTests
    {
        private MockRepository _MockRepository;

        private Mock<IAdminEntityClientAsync<Organization, int>> _MockAdminOrganizationClientAsync;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAdminOrganizationClientAsync = _MockRepository.Create<IAdminEntityClientAsync<Organization, int>>();
        }

        private ClaimsBuilder CreateClaimsBuilder()
        {
            return new ClaimsBuilder(
                _MockAdminOrganizationClientAsync.Object);
        }

        #region BuildAsync

        [TestMethod]
        public async Task ClaimsBuilder_BuildAsync_Null_ActivationCredentials_Throws()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            IActivationCredential cred = null;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await claimsBuilder.BuildAsync(cred);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ClaimsBuilder_BuildAsync_Valid()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            int organizationId = 102701;
            var cred = new ActivationCredential { Id = 27, OrganizationId = organizationId, Username = "user27", Enabled = true};
            var org = new Organization { Id = 1027, Name = "Org 1027" };
            var odataOrg = org.AsOdata<Organization, int>();
            var alternateId = new AlternateId { Entity = nameof(Organization), EntityId = organizationId.ToString(), Property = "SapId", Value = "9900001027" };
            var odataAltId = alternateId.AsOdata<AlternateId, long>();
            var relatedAltIds = new RelatedEntityCollection { Entity = nameof(Organization), RelatedEntity = nameof(AlternateId) };
            relatedAltIds.Add(odataAltId);
            odataOrg.RelatedEntityCollection.Add(relatedAltIds);
            _MockAdminOrganizationClientAsync.Setup(m => m.GetAsync(organizationId, $"$Expand={nameof(AlternateId)}", true))
                                             .ReturnsAsync(odataOrg);

            // Act
            var result = await claimsBuilder.BuildAsync(cred);

            // Assert
            Assert.AreEqual(3, result.Count);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region BuildUserClaims

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void ClaimsBuilder_BuildUserClaims_Username_NullEmptyOrWhitespace_Throws(string username)
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            string primaryIdentifier = "27";

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                claimsBuilder.BuildUserClaims(username, primaryIdentifier, DateTimeOffset.Now);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public void ClaimsBuilder_BuildUserClaims_PrimaryIdentifier_NullEmptyOrWhitespace_Throws(string primaryIdentifier)
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            string username = "user27";

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() =>
            {
                claimsBuilder.BuildUserClaims(username, primaryIdentifier, DateTimeOffset.Now);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void ClaimsBuilder_BuildUserClaims_Valid()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            string username = "user27";
            string primaryIdentifier = "27";
            var lastAuthenticated = DateTimeOffset.Now;

            // Act
            var result = claimsBuilder.BuildUserClaims(username, primaryIdentifier, lastAuthenticated);

            // Assert
            Assert.AreEqual("User", result.Subject);
            Assert.AreEqual(4, result.Claims.Count);
            Assert.AreEqual(username, result.Claims.FirstOrDefault(c => c.Name == "Username").Value);
            Assert.AreEqual(primaryIdentifier, result.Claims.FirstOrDefault(c => c.Name == "Id").Value);
            Assert.AreEqual("ActivationCredentials", result.Claims.FirstOrDefault(c => c.Name == "AuthenticationPlugin").Value);
            Assert.AreEqual(lastAuthenticated.ToString(DateTimeFormatInfo.CurrentInfo.RFC1123Pattern), result.Claims.FirstOrDefault(c => c.Name == "LastAuthenticated").Value);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region BuildOrganizationClaim

        [TestMethod]
        [PrimitiveList(0, -1, int.MinValue)]
        public async Task ClaimsBuilder_BuildOrganizationClaim_OrganizationId_NotPositiveInteger_Throws(int organizationId)
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await claimsBuilder.BuildOrganizationClaimAsync(organizationId);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ClaimsBuilder_BuildOrganizationClaim_NoOrgFound_Throws()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            int organizationId = 1027;
            var org = new Organization { Id = organizationId, Name = "Org 1027", SapId = "9900001027" };
            OdataObject<Organization, int> odataOrg = null;
            _MockAdminOrganizationClientAsync.Setup(m => m.GetAsync(organizationId, "$Expand=AlternateId", true))
                                             .ReturnsAsync(odataOrg);

            // Act
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await claimsBuilder.BuildOrganizationClaimAsync(organizationId);
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ClaimsBuilder_BuildOrganizationClaim_NoAlternateId_Valid()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            int organizationId = 1027;
            var org = new Organization { Id = organizationId, Name = "Org 1027", SapId = "9900001027" };
            var odataOrg = org.AsOdata<Organization, int>();
            _MockAdminOrganizationClientAsync.Setup(m => m.GetAsync(organizationId, "$Expand=AlternateId", true))
                                             .ReturnsAsync(odataOrg);

            // Act
            var result = await claimsBuilder.BuildOrganizationClaimAsync(organizationId);

            // Assert
            Assert.AreEqual(nameof(Organization), result.Subject);
            Assert.AreEqual(3, result.Claims.Count);
            Assert.AreEqual(organizationId.ToString(), result.Claims.FirstOrDefault(c => c.Name == "Id").Value);
            Assert.AreEqual(org.Name, result.Claims.FirstOrDefault(c => c.Name == "Name").Value);
            Assert.AreEqual(org.SapId, result.Claims.FirstOrDefault(c => c.Name == "SapId").Value);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ClaimsBuilder_BuildOrganizationClaim_AlternateId_S4Id_Valid()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            int organizationId = 1027;
            var org = new Organization { Id = organizationId, Name = "Org 1027" };
            var odataOrg = org.AsOdata<Organization, int>();
            var alternateId = new AlternateId { Entity = nameof(Organization), EntityId = organizationId.ToString(), Property = "S4Id", Value = "9900001027" };
            var odataAltId = alternateId.AsOdata<AlternateId, long>();
            var relatedAltIds = new RelatedEntityCollection { Entity = nameof(Organization), RelatedEntity = nameof(AlternateId) };
            relatedAltIds.Add(odataAltId);
            odataOrg.RelatedEntityCollection.Add(relatedAltIds);
            _MockAdminOrganizationClientAsync.Setup(m => m.GetAsync(organizationId, $"$Expand={nameof(AlternateId)}", true))
                                             .ReturnsAsync(odataOrg);

            // Act
            var result = await claimsBuilder.BuildOrganizationClaimAsync(organizationId);

            // Assert
            Assert.AreEqual(nameof(Organization), result.Subject);
            Assert.AreEqual(3, result.Claims.Count);
            Assert.AreEqual(organizationId.ToString(), result.Claims.FirstOrDefault(c => c.Name == "Id").Value);
            Assert.AreEqual(org.Name, result.Claims.FirstOrDefault(c => c.Name == "Name").Value);
            Assert.AreEqual(alternateId.Value, result.Claims.FirstOrDefault(c => c.Name == "SapId").Value);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task ClaimsBuilder_BuildOrganizationClaim_AlternateId_SapId_Valid()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();
            int organizationId = 1027;
            var org = new Organization { Id = organizationId, Name = "Org 1027" };
            var odataOrg = org.AsOdata<Organization, int>();
            var alternateId = new AlternateId { Entity = nameof(Organization), EntityId = organizationId.ToString(), Property = "SapId", Value = "9900001027" };
            var odataAltId = alternateId.AsOdata<AlternateId, long>();
            var relatedAltIds = new RelatedEntityCollection { Entity = nameof(Organization), RelatedEntity = nameof(AlternateId) };
            relatedAltIds.Add(odataAltId);
            odataOrg.RelatedEntityCollection.Add(relatedAltIds);
            _MockAdminOrganizationClientAsync.Setup(m => m.GetAsync(organizationId, $"$Expand={nameof(AlternateId)}", true))
                                             .ReturnsAsync(odataOrg);

            // Act
            var result = await claimsBuilder.BuildOrganizationClaimAsync(organizationId);

            // Assert
            Assert.AreEqual(nameof(Organization), result.Subject);
            Assert.AreEqual(3, result.Claims.Count);
            Assert.AreEqual(organizationId.ToString(), result.Claims.FirstOrDefault(c => c.Name == "Id").Value);
            Assert.AreEqual(org.Name, result.Claims.FirstOrDefault(c => c.Name == "Name").Value);
            Assert.AreEqual(alternateId.Value, result.Claims.FirstOrDefault(c => c.Name == "SapId").Value);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region BuildRolesClaim

        [TestMethod]
        public void ClaimsBuilder_BuildRolesClaim_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var claimsBuilder = CreateClaimsBuilder();

            // Act
            var result = claimsBuilder.BuildRolesClaim();

            // Assert
            Assert.AreEqual(1, result.Claims.Count);
            Assert.AreEqual("Activation", result.Claims[0].Value);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
