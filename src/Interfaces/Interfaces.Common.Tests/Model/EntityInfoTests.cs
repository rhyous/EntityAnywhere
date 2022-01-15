using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class EntityInfoTests
    {
        private EntityInfo<EntityWithManyProperties> CreateEntityInfo()
        {
            return new EntityInfo<EntityWithManyProperties>();
        }

        #region $TestedMethodName$
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var entityInfo = new EntityInfo<EntityWithManyProperties>();

            // Act
            var properties = entityInfo.Properties;

            // Assert
            Assert.AreEqual(5, properties.Count);
            Assert.IsNotNull(properties[nameof(EntityWithManyProperties.Id)]);
            Assert.IsNotNull(properties[nameof(EntityWithManyProperties.RelatedId)]);
            Assert.IsNotNull(properties[nameof(EntityWithManyProperties.Name)]);
            Assert.IsNotNull(properties[nameof(EntityWithManyProperties.Description)]);
            Assert.IsNotNull(properties[nameof(EntityWithManyProperties.CreateDate)]);
        }
        #endregion
    }
}
