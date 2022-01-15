using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    [TestClass]
    public class EntityEventWrapperTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityEventBeforeDelete<EntityInt>> _MockEntityEventBeforeDelete;
        private Mock<IEntityEventAfterDelete<EntityInt>> _MockEntityEventAfterDelete;
        private Mock<IEntityEventBeforeDeleteMany<EntityInt>> _MockEntityEventBeforeDeleteMany;
        private Mock<IEntityEventAfterDeleteMany<EntityInt, int>> _MockEntityEventAfterDeleteMany;
        private Mock<IEntityEventBeforePatch<EntityInt, int>> _MockEntityEventBeforePatch;
        private Mock<IEntityEventAfterPatch<EntityInt, int>> _MockEntityEventAfterPatch;
        private Mock<IEntityEventBeforePatchMany<EntityInt, int>> _MockEntityEventBeforePatchMany;
        private Mock<IEntityEventAfterPatchMany<EntityInt, int>> _MockEntityEventAfterPatchMany;
        private Mock<IEntityEventBeforePost<EntityInt>> _MockEntityEventBeforePost;
        private Mock<IEntityEventAfterPost<EntityInt>> _MockEntityEventAfterPost;
        private Mock<IEntityEventBeforePut<EntityInt>> _MockEntityEventBeforePut;
        private Mock<IEntityEventAfterPut<EntityInt>> _MockEntityEventAfterPut;
        private Mock<IEntityEventBeforeUpdateProperty<EntityInt>> _MockEntityEventBeforeUpdateProperty;
        private Mock<IEntityEventAfterUpdateProperty<EntityInt>> _MockEntityEventAfterUpdateProperty;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityEventBeforeDelete = _MockRepository.Create<IEntityEventBeforeDelete<EntityInt>>();
            _MockEntityEventBeforeDelete.Setup(m => m.BeforeDelete(It.IsAny<EntityInt>()));

            _MockEntityEventAfterDelete = _MockRepository.Create<IEntityEventAfterDelete<EntityInt>>();

            _MockEntityEventBeforeDeleteMany = _MockRepository.Create<IEntityEventBeforeDeleteMany<EntityInt>>();
            _MockEntityEventBeforeDeleteMany.Setup(m => m.BeforeDeleteMany(It.IsAny<IEnumerable<EntityInt>>()));

            _MockEntityEventAfterDeleteMany = _MockRepository.Create<IEntityEventAfterDeleteMany<EntityInt, int>>();

            _MockEntityEventBeforePatch = _MockRepository.Create<IEntityEventBeforePatch<EntityInt, int>>();
            _MockEntityEventBeforePatch.Setup(m => m.BeforePatch(It.IsAny<PatchedEntityComparison<EntityInt, int>>()));

            _MockEntityEventAfterPatch = _MockRepository.Create<IEntityEventAfterPatch<EntityInt, int>>();

            _MockEntityEventBeforePatchMany = _MockRepository.Create<IEntityEventBeforePatchMany<EntityInt, int>>();

            _MockEntityEventAfterPatchMany = _MockRepository.Create<IEntityEventAfterPatchMany<EntityInt, int>>();

            _MockEntityEventBeforePost = _MockRepository.Create<IEntityEventBeforePost<EntityInt>>();
            _MockEntityEventBeforePost.Setup(m => m.BeforePost(It.IsAny<List<EntityInt>>()));

            _MockEntityEventAfterPost = _MockRepository.Create<IEntityEventAfterPost<EntityInt>>();

            _MockEntityEventBeforePut = _MockRepository.Create<IEntityEventBeforePut<EntityInt>>();
            _MockEntityEventBeforePut.Setup(m => m.BeforePut(It.IsAny<EntityInt>(), It.IsAny<EntityInt>()));

            _MockEntityEventAfterPut = _MockRepository.Create<IEntityEventAfterPut<EntityInt>>();

            _MockEntityEventBeforeUpdateProperty = _MockRepository.Create<IEntityEventBeforeUpdateProperty<EntityInt>>();
            _MockEntityEventBeforeUpdateProperty.Setup(m => m.BeforeUpdateProperty(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()));

            _MockEntityEventAfterUpdateProperty = _MockRepository.Create<IEntityEventAfterUpdateProperty<EntityInt>>();

            mockLogger = _MockRepository.Create<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //mockRepository.VerifyAll();
        }

        private EntityEventContainer<EntityInt, int> CreateEntityEventContainer()
        {
            return new EntityEventContainer<EntityInt, int>(
                _MockEntityEventBeforeDelete.Object,
                _MockEntityEventAfterDelete.Object,
                _MockEntityEventBeforeDeleteMany.Object,
                _MockEntityEventAfterDeleteMany.Object,
                _MockEntityEventBeforePatch.Object,
                _MockEntityEventAfterPatch.Object,
                _MockEntityEventBeforePatchMany.Object,
                _MockEntityEventAfterPatchMany.Object,
                _MockEntityEventBeforePost.Object,
                _MockEntityEventAfterPost.Object,
                _MockEntityEventBeforePut.Object,
                _MockEntityEventAfterPut.Object,
                _MockEntityEventBeforeUpdateProperty.Object,
                _MockEntityEventAfterUpdateProperty.Object);
        }

        #region Constructor tests
        [TestMethod]
        public void EntityEventContainer_Constructor_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);

            // Act
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);

            // Assert
            Assert.IsNotNull(unitUnderTest);
        }

        [TestMethod]
        public void EntityEventContainer_Constructor_NullContainer_Test()
        {
            // Arrange
            EntityEventContainer<EntityInt, int> eventContainer = null;
            var tryLog = new TryLog(mockLogger.Object);

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            });
        }

        [TestMethod]
        public void EntityEventContainer_Constructor_NullTryLog_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            TryLog tryLog = null;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            });
        }
        #endregion

        #region Afters
        [TestMethod]
        public void EntityEventContainer_AfterDelete_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            _MockEntityEventAfterDelete.Setup(m => m.AfterDelete(It.IsAny<EntityInt>(), It.IsAny<bool>()));

            // Act
            unitUnderTest.AfterDelete(entity, true);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterDelete.Verify(m => m.AfterDelete(It.IsAny<EntityInt>(), It.IsAny<bool>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_AfterDeleteMany_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entities = new[] { new EntityInt { Id = 17 } };
            var dictionary = new Dictionary<int, bool> { { 17, true } };
            _MockEntityEventAfterDeleteMany.Setup(m => m.AfterDeleteMany(entities, dictionary));
            // Act
            unitUnderTest.AfterDeleteMany(entities, dictionary);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterDeleteMany.Verify(m => m.AfterDeleteMany(entities, dictionary), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_AfterPatch_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var patchedEntity = new PatchedEntity<EntityInt, int>();
            _MockEntityEventAfterPatch.Setup(m => m.AfterPatch(It.IsAny<PatchedEntityComparison<EntityInt, int>>()));
            var patchedEntityComparison = new PatchedEntityComparison<EntityInt, int>
            {
                PatchedEntity = patchedEntity,
                Entity = entity
            };
            // Act
            unitUnderTest.AfterPatch(patchedEntityComparison);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterPatch.Verify(m => m.AfterPatch(It.IsAny<PatchedEntityComparison<EntityInt, int>>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_AfterPatchMany_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var patchedEntity = new PatchedEntity<EntityInt, int>();
            _MockEntityEventAfterPatchMany.Setup(m => m.AfterPatchMany(It.IsAny<IEnumerable<PatchedEntityComparison<EntityInt, int>>>()));
            var patchedEntityComparison = new PatchedEntityComparison<EntityInt, int>
            {
                PatchedEntity = patchedEntity,
                Entity = entity
            };
            
            // Act
            unitUnderTest.AfterPatchMany(new[] { patchedEntityComparison });

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterPatchMany.Verify(m => m.AfterPatchMany(It.IsAny<IEnumerable<PatchedEntityComparison<EntityInt, int>>>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_AfterPost_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var entities = new List<EntityInt> { entity };
            _MockEntityEventAfterPost.Setup(m => m.AfterPost(It.IsAny<List<EntityInt>>()));

            // Act
            unitUnderTest.AfterPost(entities);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterPost.Verify(m => m.AfterPost(It.IsAny<List<EntityInt>>()));
        }

        [TestMethod]
        public void EntityEventContainer_AfterPut_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var priorEntity = new EntityInt();
            _MockEntityEventAfterPut.Setup(m => m.AfterPut(It.IsAny<EntityInt>(), It.IsAny<EntityInt>()));

            // Act
            unitUnderTest.AfterPut(entity, priorEntity);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterPut.Verify(m => m.AfterPut(It.IsAny<EntityInt>(), It.IsAny<EntityInt>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_AfterUpdateProperty_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var prop = "Prop1";
            var value = new object();
            var existingValue = new object();
            _MockEntityEventAfterUpdateProperty.Setup(m => m.AfterUpdateProperty(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()));

            // Act
            unitUnderTest.AfterUpdateProperty(prop, value, existingValue);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventAfterUpdateProperty.Verify(m => m.AfterUpdateProperty(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()), Times.Once);
        }
        #endregion

        #region Befores
        [TestMethod]
        public void EntityEventContainer_BeforeDelete_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            _MockEntityEventBeforeDelete.Setup(m => m.BeforeDelete(It.IsAny<EntityInt>()));

            // Act
            unitUnderTest.BeforeDelete(entity);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforeDelete.Verify(m => m.BeforeDelete(It.IsAny<EntityInt>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_BeforeDeleteMany_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entities = new[] { new EntityInt() };
            _MockEntityEventBeforeDeleteMany.Setup(m => m.BeforeDeleteMany(entities));

            // Act
            unitUnderTest.BeforeDeleteMany(entities);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforeDeleteMany.Verify(m => m.BeforeDeleteMany(entities), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_BeforePatch_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var patchedEntity = new PatchedEntity<EntityInt, int>();
            _MockEntityEventBeforePatch.Setup(m => m.BeforePatch(It.IsAny<PatchedEntityComparison<EntityInt, int>>()));
            var patchedEntityComparison = new PatchedEntityComparison<EntityInt, int>
            {
                PatchedEntity = patchedEntity,
                Entity = entity
            };
            // Act
            unitUnderTest.BeforePatch(patchedEntityComparison);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforePatch.Verify(m => m.BeforePatch(It.IsAny<PatchedEntityComparison<EntityInt, int>>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_BeforePatchMany_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var patchedEntity = new PatchedEntity<EntityInt, int>();
            _MockEntityEventBeforePatchMany.Setup(m => m.BeforePatchMany(It.IsAny<IEnumerable<PatchedEntityComparison<EntityInt, int>>>()));
            var patchedEntityComparison = new PatchedEntityComparison<EntityInt, int>
            {
                PatchedEntity = patchedEntity,
                Entity = entity
            };
            // Act
            unitUnderTest.BeforePatchMany(new[] { patchedEntityComparison });

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforePatchMany.Verify(m => m.BeforePatchMany(It.IsAny<IEnumerable<PatchedEntityComparison<EntityInt, int>>>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_BeforePost_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var entities = new List<EntityInt> { entity };
            _MockEntityEventBeforePost.Setup(m => m.BeforePost(It.IsAny<List<EntityInt>>()));

            // Act
            unitUnderTest.BeforePost(entities);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforePost.Verify(m => m.BeforePost(It.IsAny<List<EntityInt>>()));
        }

        [TestMethod]
        public void EntityEventContainer_BeforePut_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var entity = new EntityInt();
            var priorEntity = new EntityInt();
            _MockEntityEventBeforePut.Setup(m => m.BeforePut(It.IsAny<EntityInt>(), It.IsAny<EntityInt>()));

            // Act
            unitUnderTest.BeforePut(entity, priorEntity);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforePut.Verify(m => m.BeforePut(It.IsAny<EntityInt>(), It.IsAny<EntityInt>()), Times.Once);
        }

        [TestMethod]
        public void EntityEventContainer_BeforeUpdateProperty_Test()
        {
            // Arrange
            var eventContainer = CreateEntityEventContainer();
            var tryLog = new TryLog(mockLogger.Object);
            var unitUnderTest = new EntityEventWrapper<EntityInt, int>(eventContainer, tryLog);
            var prop = "Prop1";
            var value = new object();
            var existingValue = new object();
            _MockEntityEventBeforeUpdateProperty.Setup(m => m.BeforeUpdateProperty(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()));

            // Act
            unitUnderTest.BeforeUpdateProperty(prop, value, existingValue);

            // Assert
            Assert.IsNotNull(unitUnderTest);
            _MockEntityEventBeforeUpdateProperty.Verify(m => m.BeforeUpdateProperty(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<object>()), Times.Once);
        }
        #endregion
    }
}