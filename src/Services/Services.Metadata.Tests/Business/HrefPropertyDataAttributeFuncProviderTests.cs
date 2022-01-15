using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services.Tests.Business
{
    [TestClass]
    public class HrefPropertyDataAttributeFuncProviderTests
    {
        [TestMethod]
        public void HrefPropertyDataAttributeFuncProvider_GetPropertyData_NoAttribute_Test()
        {            // Arrange
            var provider = new HrefPropertyDataAttributeFuncProvider();
            var type = typeof(EntityInt);

            // Act
            var kvp = provider.GetPropertyData(type)?.FirstOrDefault();

            // Assert
            Assert.IsNull(kvp);
        }

        [TestMethod]
        public void HrefPropertyDataAttributeFuncProvider_GetPropertyData_AttributeExists_Test()
        {
            // Arrange
            var provider = new HrefPropertyDataAttributeFuncProvider();
            var type = typeof(EntityString);
            var expectedKey = "@StringType";
            var expectedValue = "href";

            // Act
            var kvp = provider.GetPropertyData(type.GetProperty("Url"))?.FirstOrDefault();

            // Assert
            Assert.IsNotNull(kvp);
            Assert.AreEqual(expectedKey, kvp.Value.Key);
            Assert.AreEqual(expectedValue, kvp.Value.Value);
        }
    }
}
