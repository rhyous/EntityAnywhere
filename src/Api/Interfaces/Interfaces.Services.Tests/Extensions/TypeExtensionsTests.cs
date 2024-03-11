using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Interfaces.Tests.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        
        [TestMethod]
        public void GetMappedEntity1PropertyInfo_Test()
        {
            // Arrange
            Type t = typeof(UserTypeMap);

            // Act
            var result = t.GetMappedEntity1PropertyInfo();

            // Assert
            Assert.AreEqual("UserTypeId", result.Name);
        }

        [TestMethod]
        public void GetMappedEntity2PropertyInfo_Test()
        {
            // Arrange
            Type t = typeof(UserTypeMap);

            // Act
            var result = t.GetMappedEntity2PropertyInfo();

            // Assert
            Assert.AreEqual("UserId", result.Name);
        }
    }
}
