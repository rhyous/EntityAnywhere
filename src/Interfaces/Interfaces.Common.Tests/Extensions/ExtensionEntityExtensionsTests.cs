using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Entities;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class ExtensionEntityExtensionsTests
    {   
        [TestMethod]
        public void ExtensionEntityExtensions_Get_ReturnsValueWhenExists_CaseInsensityProperty_Test()
        {
            // Arrange
            var addenda = new List<Addendum>
            {
                new Addendum{ Id = 11, Entity="Organization", EntityId = "1101", Property = "Prop1.1", Value = "Val1.1" },
                new Addendum{ Id = 12, Entity="Organization", EntityId = "1101", Property = "Prop1.2", Value = "Val1.2" },
                new Addendum{ Id = 13, Entity="Organization", EntityId = "1102", Property = "Prop2.1", Value = "Val2.1" },
                new Addendum{ Id = 14, Entity="Organization", EntityId = "1102", Property = "Prop2.2", Value = "Val2.2" },
            };

            // Act
            var actual = addenda.Get("Organization", "1101", "prop1.1", "defaultvalue");

            // Assert
            Assert.AreEqual("Val1.1", actual);
        }


        [TestMethod]
        public void ExtensionEntityExtensions_Get_ReturnsDefaultValueWhenNotExists_CaseInsensityProperty_Test()
        {
            // Arrange
            var addenda = new List<Addendum>
            {
                new Addendum{ Id = 11, Entity="Organization", EntityId = "1101", Property = "Prop1.1", Value = "Val1.1" },
                new Addendum{ Id = 12, Entity="Organization", EntityId = "1101", Property = "Prop1.2", Value = "Val1.2" },
                new Addendum{ Id = 13, Entity="Organization", EntityId = "1102", Property = "Prop2.1", Value = "Val2.1" },
                new Addendum{ Id = 14, Entity="Organization", EntityId = "1102", Property = "Prop2.2", Value = "Val2.2" },
            };

            // Act
            var actual = addenda.Get("Organization", "1101", "prop1.3", "defaultvalue");

            // Assert
            Assert.AreEqual("defaultvalue", actual);
        }
    }
}
