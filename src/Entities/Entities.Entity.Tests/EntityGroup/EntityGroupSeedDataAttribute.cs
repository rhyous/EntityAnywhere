using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Entities.Tests
{
    [TestClass]
    public class SeedDataTests
    {
        [TestMethod]
        public void Have_Seeded_Data_Correctly()
        {
            // Arrange
            var attr = new EntityGroupSeedDataAttribute();


            // Act
            var result = attr.Objects;


            // Assert
            Assert.AreEqual(10, result.Count);
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Miscellaneous"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Entitlement Management"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Entity Management"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Extension Entities"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Licensing"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Naurtech Management"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Product Packaging Management"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "Organization Management"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "User Management"));
            Assert.IsNotNull(result.FirstOrDefault(x => ((EntityGroup)x).Name == "System Configuration"));
        }
    }
}
