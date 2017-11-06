using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Services;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Clients;
using Moq;
using System.Collections.Generic;

namespace Services.Authentication.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BuildTest()
        {
            // Arrange
            var entity = "Organization";
            var builder = new ClaimsBuilderAsync();
            var mockClient = new Mock<IEntityClientAsync>();
            var json = "";
            mockClient.Setup(c => c.GetAllAsync(It.IsAny<string>())).ReturnsAsync(json);
            var client = mockClient.Object;
            builder.ClientsCache.Json.Add(entity, client);
            IUser user = new User { Id = 1, Username = "TestUser1" };
            var claimConfigurations = new List<ClaimConfiguration>
            {
                new ClaimConfiguration { Name = "Username", Entity = "User", EntityProperty = "Username", UserIdProperty = "Id" }
            };

            // Act
            var actual = TaskRunner.RunSynchonously(builder.BuildAsync, user, claimConfigurations);

            // Assert
            Assert.AreEqual(1, actual);
            Assert.AreEqual(1, actual[0].Claims);
            Assert.AreEqual(1, actual[0].Claims[0].Name = "Username");
            Assert.AreEqual(1, actual[0].Claims[0].Value = "TestUser1");
            Assert.AreEqual(1, actual[0].Claims[0].Issuer = "LOCAL AUTHORITY");
        }
    }
}