using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Clients2;
using System;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests.Models
{
    [TestClass]
    public class MappingEntitySettingsTests
    {
        [TestMethod]
        public void MappingEntitySettings_Constructor_Throws_Test()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                new MappingEntitySettings<EntityInt>();
            });
        }

        [TestMethod]
        public void MappingEntitySettings_Constructor_Test()
        {
            // Arrange
            // Act
            var mappingEntitySettings = new MappingEntitySettings<MappingEntityInt>();

            // Assert
            Assert.AreEqual(nameof(EntityInt), mappingEntitySettings.Entity1);
            Assert.AreEqual(nameof(EntityInt)+"s", mappingEntitySettings.Entity1Pluralized);
            Assert.AreEqual("EntityIntId", mappingEntitySettings.Entity1Property);
            Assert.AreEqual(nameof(EntityString), mappingEntitySettings.Entity2);
            Assert.AreEqual(nameof(EntityString) + "s", mappingEntitySettings.Entity2Pluralized);
            Assert.AreEqual("EntityStringId", mappingEntitySettings.Entity2Property);
        }
    }
}
