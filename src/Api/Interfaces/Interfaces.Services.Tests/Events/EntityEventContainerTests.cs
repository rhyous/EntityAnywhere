using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    [TestClass]
    public class EntityEventContainerTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityEventBeforeDelete<EntityInt, int>> _MockEntityEventBeforeDelete;
        private Mock<IEntityEventAfterDelete<EntityInt, int>> _MockEntityEventAfterDelete;
        private Mock<IEntityEventBeforeDeleteMany<EntityInt, int>> _MockEntityEventBeforeDeleteMany;
        private Mock<IEntityEventAfterDeleteMany<EntityInt, int>> _MockEntityEventAfterDeleteMany;
        private Mock<IEntityEventBeforePatch<EntityInt, int>> _MockEntityEventBeforePatch;
        private Mock<IEntityEventAfterPatch<EntityInt, int>> _MockEntityEventAfterPatch;
        private Mock<IEntityEventBeforePatchMany<EntityInt, int>> _MockEntityEventBeforePatchMany;
        private Mock<IEntityEventAfterPatchMany<EntityInt, int>> _MockEntityEventAfterPatchMany;
        private Mock<IEntityEventBeforePost<EntityInt, int>> _MockEntityEventBeforePost;
        private Mock<IEntityEventAfterPost<EntityInt, int>> _MockEntityEventAfterPost;
        private Mock<IEntityEventBeforePut<EntityInt, int>> _MockEntityEventBeforePut;
        private Mock<IEntityEventAfterPut<EntityInt, int>> _MockEntityEventAfterPut;
        private Mock<IEntityEventBeforeUpdateProperty<EntityInt, int>> _MockEntityEventBeforeUpdateProperty;
        private Mock<IEntityEventAfterUpdateProperty<EntityInt, int>> _MockEntityEventAfterUpdateProperty;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityEventBeforeDelete = _MockRepository.Create<IEntityEventBeforeDelete<EntityInt, int>>();
            _MockEntityEventAfterDelete = _MockRepository.Create<IEntityEventAfterDelete<EntityInt, int>>();
            _MockEntityEventBeforeDeleteMany = _MockRepository.Create<IEntityEventBeforeDeleteMany<EntityInt, int>>();
            _MockEntityEventAfterDeleteMany = _MockRepository.Create<IEntityEventAfterDeleteMany<EntityInt, int>>();
            _MockEntityEventBeforePatch = _MockRepository.Create<IEntityEventBeforePatch<EntityInt, int>>();
            _MockEntityEventAfterPatch = _MockRepository.Create<IEntityEventAfterPatch<EntityInt, int>>();
            _MockEntityEventBeforePatchMany = _MockRepository.Create<IEntityEventBeforePatchMany<EntityInt, int>>();
            _MockEntityEventAfterPatchMany = _MockRepository.Create<IEntityEventAfterPatchMany<EntityInt, int>>();
            _MockEntityEventBeforePost = _MockRepository.Create<IEntityEventBeforePost<EntityInt, int>>();
            _MockEntityEventAfterPost = _MockRepository.Create<IEntityEventAfterPost<EntityInt, int>>();
            _MockEntityEventBeforePut = _MockRepository.Create<IEntityEventBeforePut<EntityInt, int>>();
            _MockEntityEventAfterPut = _MockRepository.Create<IEntityEventAfterPut<EntityInt, int>>();
            _MockEntityEventBeforeUpdateProperty = _MockRepository.Create<IEntityEventBeforeUpdateProperty<EntityInt, int>>();
            _MockEntityEventAfterUpdateProperty = _MockRepository.Create<IEntityEventAfterUpdateProperty<EntityInt, int>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
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

        private EntityEventContainer<EntityInt, int> CreateEntityEventContainerFromDictionary()
        {
            var dict = new Dictionary<Type, IEntityEvent<EntityInt, int>>
            {
                { typeof(IEntityEventBeforeDelete<EntityInt, int>), _MockEntityEventBeforeDelete.Object },
                { typeof(IEntityEventAfterDelete<EntityInt, int>), _MockEntityEventAfterDelete.Object },
                { typeof(IEntityEventBeforeDeleteMany<EntityInt, int>), _MockEntityEventBeforeDeleteMany.Object },
                { typeof(IEntityEventAfterDeleteMany<EntityInt, int>), _MockEntityEventAfterDeleteMany.Object },
                { typeof(IEntityEventBeforePatch<EntityInt, int>), _MockEntityEventBeforePatch.Object },
                { typeof(IEntityEventAfterPatch<EntityInt, int>), _MockEntityEventAfterPatch.Object },
                { typeof(IEntityEventBeforePatchMany<EntityInt, int>), _MockEntityEventBeforePatchMany.Object },
                { typeof(IEntityEventAfterPatchMany<EntityInt, int>), _MockEntityEventAfterPatchMany.Object },
                { typeof(IEntityEventBeforePost<EntityInt, int>), _MockEntityEventBeforePost.Object },
                { typeof(IEntityEventAfterPost<EntityInt, int>), _MockEntityEventAfterPost.Object },
                { typeof(IEntityEventBeforePut<EntityInt, int>), _MockEntityEventBeforePut.Object },
                { typeof(IEntityEventAfterPut<EntityInt, int>), _MockEntityEventAfterPut.Object },
                { typeof(IEntityEventBeforeUpdateProperty<EntityInt, int>), _MockEntityEventBeforeUpdateProperty.Object },
                { typeof(IEntityEventAfterUpdateProperty<EntityInt, int>), _MockEntityEventAfterUpdateProperty.Object  }
            };

            return new EntityEventContainer<EntityInt, int>(dict, new EntityEventTypes<EntityInt, int>());
        }

        [TestMethod]
        public void EntityEventContainer_Constructor_Test()
        {
            // Arrange
            // Act
            var unitUnderTest = CreateEntityEventContainer();

            // Assert
            Assert.IsNotNull(unitUnderTest);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforeDelete);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterDelete);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforeDeleteMany);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterDeleteMany);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforePatch);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterPatch);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforePost);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterPost);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforePut);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterPut);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforeUpdateProperty);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterUpdateProperty);
        }

        [TestMethod]
        public void EntityEventContainer_Constructor_Dictionary_Test()
        {
            // Arrange
            // Act
            var unitUnderTest = CreateEntityEventContainerFromDictionary();

            // Assert
            Assert.IsNotNull(unitUnderTest);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforeDelete);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterDelete);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforeDeleteMany);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterDeleteMany);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforePatch);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterPatch);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforePost);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterPost);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforePut);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterPut);
            Assert.IsNotNull(unitUnderTest.EntityEventBeforeUpdateProperty);
            Assert.IsNotNull(unitUnderTest.EntityEventAfterUpdateProperty);
        }
    }
}
