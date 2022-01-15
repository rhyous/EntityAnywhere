using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class ClaimsBuilderAsyncTests
    {
        #region BuildUser
        [TestMethod]
        public async Task ClaimConfiguration_BuildUser_Test()
        {
            // Arrange
            var builder = new ClaimsBuilderAsync(Mock.Of<IAdminEntityClientAsync<ClaimConfiguration, int>>(), Mock.Of<IAdminEntityClientAsync<User, long>>());

            // Act
            var actual = await builder.BuildAsync(Data.User, Data.ClaimConfigurationListUser, (IList<RelatedEntityCollection>)null);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual[0].Claims.Count);
            Assert.AreEqual("User", actual[0].Claims[0].Domain.Subject);
            Assert.AreEqual("Username", actual[0].Claims[0].Name);
            Assert.AreEqual("TestUser1", actual[0].Claims[0].Value);
            Assert.AreEqual("LOCAL AUTHORITY", actual[0].Claims[0].Issuer);
        }

        [TestMethod]
        public async Task ClaimConfiguration_BuildFromRelatedEnity_Role_Test()
        {
            // Arrange
            var builder = new ClaimsBuilderAsync(Mock.Of<IAdminEntityClientAsync<ClaimConfiguration, int>>(), Mock.Of<IAdminEntityClientAsync<User, long>>());

            // Act
            var actual = await builder.BuildAsync(Data.User, Data.ClaimConfigurationListRole, Data.RelatedEntityCollectionGroupAndRole);

            // Assert
            Assert.AreEqual(2, actual.Count);
            int id = 1;
            Assert.AreEqual(2, actual[id].Claims.Count);

            Assert.AreEqual("UserRole", actual[id].Claims[0].Domain.Subject);
            Assert.AreEqual("Role", actual[id].Claims[0].Name);
            Assert.AreEqual("Role1", actual[id].Claims[0].Value);
            Assert.AreEqual("LOCAL AUTHORITY", actual[0].Claims[0].Issuer);
            
            Assert.AreEqual("UserRole", actual[id].Claims[1].Domain.Subject);
            Assert.AreEqual("Role", actual[id].Claims[1].Name);
            Assert.AreEqual("Role2", actual[id].Claims[1].Value);
            Assert.AreEqual("LOCAL AUTHORITY", actual[id].Claims[1].Issuer);
        }

        [TestMethod]
        public async Task ClaimConfiguration_BuildFromRelatedEntity_LandingPage_Test()
        {
            // Arrange
            var builder = new ClaimsBuilderAsync(Mock.Of<IAdminEntityClientAsync<ClaimConfiguration, int>>(), Mock.Of<IAdminEntityClientAsync<User, long>>());

            // Act
            var actual = await builder.BuildAsync(Data.User, new List<ClaimConfiguration> { Data.ClaimConfigurationLandingPageType }, Data.RelatedEntityCollectionGroupAndRole);

            // Assert           
            int id = 1;

            Assert.AreEqual(nameof(UserRole.LandingPageId), actual[id].Claims[0].Name);
            Assert.AreEqual("1", actual[id].Claims[0].Value);

            Assert.AreEqual(nameof(UserRole.LandingPageId), actual[id].Claims[1].Name);
            Assert.AreEqual("2", actual[id].Claims[1].Value);
        }

        [TestMethod]
        public async Task ClaimConfiguration_BuildFromRelatedEnity_Group_Test()
        {
            // Arrange
            var builder = new ClaimsBuilderAsync(Mock.Of<IAdminEntityClientAsync<ClaimConfiguration, int>>(), Mock.Of<IAdminEntityClientAsync<User, long>>());

            // Act
            var actual = await builder.BuildAsync(Data.User, Data.ClaimConfigurationListGroup, Data.RelatedEntityCollectionGroupAndRole);

            // Assert
            Assert.AreEqual(2, actual.Count);
            int id = 1;
            Assert.AreEqual(1, actual[id].Claims.Count);
            Assert.AreEqual("UserGroup", actual[id].Claims[0].Domain.Subject);
            Assert.AreEqual("Group", actual[id].Claims[0].Name);
            Assert.AreEqual("Group2", actual[id].Claims[0].Value);
            Assert.AreEqual("LOCAL AUTHORITY", actual[id].Claims[0].Issuer);
        }
        #endregion

        #region GetUserClaims
        [TestMethod]
        public void ClaimConfiguration_BuildUserClaims_None_Test()
        {
            // Arrange
            var builder = new ClaimsBuilderAsync(Mock.Of<IAdminEntityClientAsync<ClaimConfiguration, int>>(), Mock.Of<IAdminEntityClientAsync<User, long>>());

            var dict = new ClaimDomainDictionary();
            dict.Add("User", new ClaimDomain { Subject = "User", Issuer = "LOCAL AUTHORITY" });
            var userDomain = dict["User"];

            // Act
            builder.BuildClaimsFromUserEntity(dict, Data.User, null);

            // Assert
            Assert.AreEqual(1, userDomain.Claims.Count);
            Assert.AreEqual("User", userDomain.Claims[0].Domain.Subject);
            Assert.AreEqual("LastAuthenticated", userDomain.Claims[0].Name);
            Assert.AreEqual("LOCAL AUTHORITY", userDomain.Claims[0].Issuer);
        }

        [TestMethod]
        public void ClaimConfiguration_BuildUserClaims_UserName_Test()
        {
            // Arrange
            var builder = new ClaimsBuilderAsync(Mock.Of<IAdminEntityClientAsync<ClaimConfiguration, int>>(), Mock.Of<IAdminEntityClientAsync<User, long>>());

            var dict = new ClaimDomainDictionary();
            dict.Add("User", new ClaimDomain { Subject = "User", Issuer = "LOCAL AUTHORITY" });
            var userDomain = dict["User"];

            // Act
            builder.BuildClaimsFromUserEntity(dict, Data.User, Data.ClaimConfigurationListUser);

            // Assert
            Assert.AreEqual(2, userDomain.Claims.Count);
            Assert.AreEqual("User", userDomain.Claims[0].Domain.Subject);
            Assert.AreEqual("Username", userDomain.Claims[0].Name);
            Assert.AreEqual("TestUser1", userDomain.Claims[0].Value);
            Assert.AreEqual("LOCAL AUTHORITY", userDomain.Claims[0].Issuer);
        }

        #endregion
    }
}