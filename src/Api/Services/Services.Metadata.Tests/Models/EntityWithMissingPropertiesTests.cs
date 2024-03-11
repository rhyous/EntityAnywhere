using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Collections;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services.Tests.Models
{
    [TestClass]
    public class EntityWithMissingPropertiesTests
    {
        private EntityWithMissingProperties CreateEntityWithMissingProperties()
        {
            return new EntityWithMissingProperties();
        }

        #region $TestedMethodName$
        [TestMethod]
        public void EntityWithMissingProperties_SearchableProperties_DefaultValues_Test()
        {
            // Arrange
            var entityWithMissingProperties = this.CreateEntityWithMissingProperties();

            // Act
            var actual = entityWithMissingProperties.SearchableProperties;

            // Assert
            Assert.AreEqual("Id", actual.First());
            Assert.AreEqual("Name", actual.Second());
        }
        #endregion
    }
}
