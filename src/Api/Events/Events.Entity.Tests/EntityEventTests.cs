using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Interfaces.Common.Tests;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Events.Tests
{
    [TestClass]
    public class EntityEventTests
    {
        private MockRepository _MockRepository;

        private Mock<IMetadataCache> _MockMetadataCache;
        private Mock<IEntitySettingsCache> _MockEntitySettingsCache;
        private Mock<ILogger> _MockLogger;

        private const string DataChangedLog = "Entity configuration data changed. Clearing metadata and entitysettings.";

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockMetadataCache = _MockRepository.Create<IMetadataCache>();
            _MockEntitySettingsCache = _MockRepository.Create<IEntitySettingsCache>();
            _MockLogger = _MockRepository.Create<ILogger>();
        }

        private EntityEvent CreateEntityEvent()
        {
            return new EntityEvent(
                _MockMetadataCache.Object,
                _MockEntitySettingsCache.Object,
                _MockLogger.Object);
        }

        #region AfterDelete
        [TestMethod]
        public void EntityEvent_AfterDelete_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();
            Entity entity = new Entity { Id = 10, Name = nameof(EntityInt)};
            bool wasDeleted = false;
            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));
            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterDelete(entity, wasDeleted);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterDeleteMany
        [TestMethod]
        public void EntityEvent_AfterDeleteMany_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();
            Entity entity1 = new Entity { Id = 10, Name = nameof(EntityInt) };
            Entity entity2 = new Entity { Id = 11, Name = nameof(EntityString) };
            IEnumerable<Entity> entities = new List<Entity> { entity1, entity2 };
            Dictionary<int, bool> wasDeleted = null;

            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));

            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterDeleteMany(entities, wasDeleted);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterPatch
        [TestMethod]
        public void EntityEvent_AfterPatch_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();
            Entity entity = new Entity { Id = 10, Name = nameof(EntityInt) };
            var patchedEntityComparison = new PatchedEntityComparison<Entity, int>
            { 
                Entity = entity
            };
            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));
            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterPatch(patchedEntityComparison);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterPatchMany
        [TestMethod]
        public void EntityEvent_AfterPatchMany_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();

            Entity entity1 = new Entity { Id = 10, Name = nameof(EntityInt) };
            var patchedEntityComparison1 = new PatchedEntityComparison<Entity, int>
            {
                Entity = entity1
            };

            Entity entity2 = new Entity { Id = 11, Name = nameof(EntityString) };
            var patchedEntityComparison2 = new PatchedEntityComparison<Entity, int>
            {
                Entity = entity2
            };
            var patchedEntityComparisons = new List<PatchedEntityComparison<Entity, int>>
            {
                patchedEntityComparison1, patchedEntityComparison2
            };

            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));
            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterPatchMany(patchedEntityComparisons);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterPost
        [TestMethod]
        public void EntityEvent_AfterPost_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();

            Entity entity1 = new Entity { Id = 10, Name = nameof(EntityInt) };
            Entity entity2 = new Entity { Id = 11, Name = nameof(EntityString) };
            IEnumerable<Entity> entities = new List<Entity> { entity1, entity2 };

            IEnumerable<Entity> postedItems = new List<Entity> { entity1, entity2 };

            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));

            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterPost(postedItems);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterPut
        [TestMethod]
        public void EntityEvent_AfterPut_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();
            Entity entity = new Entity { Id = 10, Name = nameof(EntityInt) };
            Entity priorEntity = null;

            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));
            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterPut(entity, priorEntity);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region AfterUpdateProperty
        [TestMethod]
        public void EntityEvent_AfterUpdateProperty_CachesCleared_Test()
        {
            // Arrange
            var entityEvent = CreateEntityEvent();
            string property = null;
            object newValue = null;
            object existingValue = null;
            _MockLogger.Setup(m => m.Debug(DataChangedLog, nameof(EntityEvent.ClearEntityMetadata), It.Is<string>(s => s.EndsWith("EntityEvent.cs")), It.IsAny<int>()));
            IMetadataDictionary metadataDictionary = null;
            _MockMetadataCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(metadataDictionary);
            IEntitySettingsDictionary entitySettingsDictionary = null;
            _MockEntitySettingsCache.Setup(m => m.ProvideAsync(true)).ReturnsAsync(entitySettingsDictionary);

            // Act
            entityEvent.AfterUpdateProperty(property, newValue, existingValue);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
