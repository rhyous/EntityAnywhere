using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [TestClass]
    public class JsonClaimsParserTests
    {
        [TestMethod]
        public void ParseTest()
        {
            // Arrange
            var relatedEntity = new RelatedEntity
            {
                Id = "16485",                
                Object = new JRaw("{\"Id\":16485,\"Name\":\"Role1\"}")
            };
            var relatedEntityCollection = new RelatedEntityCollection { RelatedEntity = "UserRole" };
            relatedEntityCollection.Add(relatedEntity);
            var claimConfig = new ClaimConfiguration { Id = 4, Domain = "Organization", Name = "Name", Entity = "Organization", EntityProperty = "Name", EntityIdProperty = "Id", RelatedEntityIdProperty = "OrganizationId" };

            // Act
            var actual = JsonClaimsParser.Parse(claimConfig, relatedEntityCollection);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("Role1", actual.First());
        }
    }
}