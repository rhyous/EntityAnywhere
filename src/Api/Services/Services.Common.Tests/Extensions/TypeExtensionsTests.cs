using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;

namespace Services.Common.Tests
{
    #region Some classes to help with generic testing
    public class Test1Attribute : Attribute
    {
        public string TestProperty { get; set; }
    }

    [Test1(TestProperty = "Test Value")]
    public class Test1Class { }

    [AlternateKey("Username")]
    public class User : IBaseEntity<int>
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
    #endregion

    [TestClass]
    public class TypeExtensionsTests
    {
        #region GetAlternateIdProperty

        [TestMethod]
        public void TypeExtensions_GetAlternateIdProperty_TypeNull_Test()
        {
            // Arrange - Done above
            // Act
            Type type = null;
            var value = type.GetAlternateKeyProperty();

            // Assert
            Assert.IsNull(value);
        }

        [TestMethod]
        public void TypeExtensions_GetAlternateIdProperty_TypeHasNoAttribute_Test()
        {
            // Arrange - Done above
            // Act
            Type type = typeof(string);
            var value = type.GetAlternateKeyProperty();

            // Assert
            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetAlternateIdPropertyValueTests()
        {
            // Arrange - Done above
            // Act
            var value = typeof(User).GetAlternateKeyProperty();

            // Assert
            Assert.AreEqual("Username", value);
        }

        #endregion

        #region GetEditableProperties

        [TestMethod]
        public void TypeExtensions_GetEditableProperties_TypeNull_ReturnsNull()
        {
            // Arrange - Done above
            // Act
            Type type = null;
            var value = type.GetEditableProperties();

            // Assert
            Assert.IsNull(value);
        }

        [TestMethod]
        public void TypeExtensions_GetEditableProperties_TypeHasNoEditableProperties_ReturnsNull()
        {
            // Arrange - Done above
            // Act
            Type type = typeof(IAuditable);
            var value = type.GetEditableProperties();

            // Assert
            Assert.IsTrue(!value.Any());
        }

        [TestMethod]
        public void TypeExtensions_GetEditableProperties_TypeHasEditableProperties_ReturnsProperyList()
        {
            // Arrange - Done above
            // Act
            Type type = typeof(IId<int>);
            var value = type.GetEditableProperties();

            // Assert
            Assert.AreEqual(type.GetProperties().Count(), value.Count());
        }

        #endregion
    }
}
