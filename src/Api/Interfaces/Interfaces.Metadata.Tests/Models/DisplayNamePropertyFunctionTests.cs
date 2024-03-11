using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Interfaces.Common.Tests;
using System.Linq;

namespace Rhyous.EntityAnywhere.Metadata.Tests
{
    [TestClass]
    public class DisplayNamePropertyFunctionTests
    {
        [TestMethod]
        public void CustomMetadataProvider_GetDisplayNameProperty_Null_Test()
        {            // Arrange
            var provider = new DisplayNamePropertyFunction();
            var type = typeof(EntityInt);

            // Act
            var kvp = provider.GetDisplayNameProperty(null)?.FirstOrDefault();

            // Assert
            Assert.IsNull(kvp);
        }

        [TestMethod]
        public void CustomMetadataProvider_GetDisplayNameProperty_NoAttribute_Test()
        {            // Arrange
            var provider = new DisplayNamePropertyFunction();
            var type = typeof(EntityInt);

            // Act
            var kvp = provider.GetDisplayNameProperty(type)?.FirstOrDefault();

            // Assert
            Assert.IsNull(kvp);
        }

        [TestMethod]
        public void CustomMetadataProvider_GetDisplayNameProperty_AttributeExists_Test()
        {
            // Arrange
            var provider = new DisplayNamePropertyFunction();
            var type = typeof(EntityString);
            var expectedKey = "@UI.DisplayName";

            // Act
            var kvp = provider.GetDisplayNameProperty(type)?.FirstOrDefault();

            // Assert
            Assert.IsNotNull(kvp);
            Assert.AreEqual(expectedKey, kvp.Value.Key);
            Assert.AreEqual("Name", (kvp.Value.Value as CsdlPropertyPath).PropertyPath);
        }
    }
}
