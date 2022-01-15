using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{

    [TestClass]
    public class EntityEventTypesTests
    {        
        [TestMethod]
        public void EntityEventTypes_Constructor_Test()
        {
            // Arrange
            // Act
            var entityEventTypes = new EntityEventTypes<EntityInt, int>();

            // Assert
            int i = 0;
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforeDelete<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterDelete<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforeDeleteMany<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterDeleteMany<EntityInt, int>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforePatch<EntityInt, int>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterPatch<EntityInt, int>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforePatchMany<EntityInt, int>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterPatchMany<EntityInt, int>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforePost<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterPost<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforePut<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterPut<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventBeforeUpdateProperty<EntityInt>));
            Assert.AreEqual(entityEventTypes.Types[i++], typeof(IEntityEventAfterUpdateProperty<EntityInt>));
        }
    }
}
