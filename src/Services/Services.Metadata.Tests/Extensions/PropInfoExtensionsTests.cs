using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Entities;
using System;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services.Tests.Extensions
{
    [TestClass]
    public class PropInfoExtensionsTests
    {
        [TestMethod]
        public void PropInfoExtensions_ToEntityProperty_NullPropInfo_Test()
        {
            // Arrange
            PropertyInfo propInfo = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => 
            {
                var result = propInfo.ToEntityProperty(27);
            });
        }

        [TestMethod]
        public void PropInfoExtensions_ToEntityProperty_Test()
        {
            // Arrange
            PropertyInfo propInfo = typeof(Entity).GetProperty("Name");

            // Act
            var result = propInfo.ToEntityProperty(27);

            // Assert
            Assert.AreEqual(27, result.EntityId);
            Assert.AreEqual("Name", result.Name);
            Assert.AreEqual("System.String", result.Type);
            Assert.AreEqual(int.MaxValue, result.Order);
        }

        [TestMethod]
        public void PropInfoExtensions_ToEntityProperty_DateTime_Test()
        {
            // Arrange
            PropertyInfo propInfo = typeof(Entity).GetProperty("CreateDate");

            // Act
            var result = propInfo.ToEntityProperty(27);

            // Assert
            Assert.AreEqual(27, result.EntityId);
            Assert.AreEqual("CreateDate", result.Name);
            Assert.AreEqual("System.DateTimeOffset", result.Type);
            Assert.AreEqual(int.MaxValue, result.Order);
        }

        [TestMethod]
        public void PropInfoExtensions_ToEntityProperty_NullableInt_Test()
        {
            // Arrange
            PropertyInfo propInfo = typeof(EntityIntNullable).GetProperty("OptionalId");

            // Act
            var result = propInfo.ToEntityProperty(27);

            // Assert
            Assert.AreEqual(27, result.EntityId);
            Assert.AreEqual("OptionalId", result.Name);
            Assert.AreEqual("System.Int32", result.Type);
            Assert.AreEqual(int.MaxValue, result.Order);
            Assert.IsTrue(result.Nullable);
        }
    }
}
