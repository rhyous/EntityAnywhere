using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests.Extensions
{
    [TestClass]
    public class EntityExtensionsTests
    {
        #region ToEntityWithMissingProperties
        [TestMethod]
        public void EntityExtensions_ToEntityWithMissingProperties_Id_And_AltKey_Searchable_True()
        {
            // Arrange
            EntitySettings entitySettings = new EntitySettings { Entity = new Entity() };
            var entityType = typeof(EntityString);

            // Act
            var result = entitySettings.ToEntityWithMissingProperties(entityType);

            // Assert
            Assert.AreEqual(entitySettings.Entity, result.Entity);
            Assert.IsTrue(result.SearchableProperties.Contains(nameof(EntityString.Id)));
            Assert.IsTrue(result.SearchableProperties.Contains(nameof(EntityString.Name)));
        }
        #endregion
    }
}
