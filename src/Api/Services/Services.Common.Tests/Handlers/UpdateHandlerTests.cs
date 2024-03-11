using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class UpdateHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;
        private Mock<IEntityInfo<TEntity>> _MockEntityInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
            _MockEntityInfo = _MockRepository.Create<IEntityInfo<TEntity>>();
        }

        private UpdateHandler<TEntity, TInterface, TId> CreateUpdateHandler()
        {
            return new UpdateHandler<TEntity, TInterface, TId>(
                _MockEntityRepo.Object,
                _MockEntityInfo.Object);
        }

        #region Update
        [TestMethod]
        public void UpdateHandler_Update_IdUpdated_RepoCalled_Test()
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            TId id = 90;
            var originalEntity = new TEntity();
            PatchedEntity<TInterface, TId> patchedEntity = new PatchedEntity<TInterface, TId> { Entity = originalEntity };

            TInterface returnedEntity = new TEntity();
            _MockEntityRepo.Setup(m => m.Update(patchedEntity, false))
                           .Returns(returnedEntity);

            // Act
            var result = updateHandler.Update(id, patchedEntity);

            // Assert
            Assert.AreEqual(id, originalEntity.Id);
            Assert.AreEqual(result, returnedEntity);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update
        [TestMethod]
        public void UpdateHandler_Update_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            PatchedEntityCollection<TInterface, TId> patchedEntityCollection = null;
            List<TInterface> entityList = null;
            _MockEntityRepo.Setup(m => m.BulkUpdate(patchedEntityCollection, false))
                           .Returns(entityList);

            // Act
            var result = updateHandler.Update(patchedEntityCollection);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Update
        [TestMethod]
        [ListTNullOrEmpty(typeof(TInterface))]
        public void UpdateHandler_Update_EntitiesNullOrEmpty_Test(List<TInterface> entities)
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            var entity1 = new TEntity();
            string[] changedProperties = null;

            // Act
            var result = updateHandler.Update(entities, changedProperties);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [ArrayNullOrEmpty(typeof(string))]
        public void UpdateHandler_Update_ChangedPropertiesNullOrEmpty_Test(string[] changedProperties)
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            var entity1 = new TEntity();
            var entities = new List<TInterface> { entity1 };

            // Act
            var result = updateHandler.Update(entities, changedProperties);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateHandler_Update_StateUnderTest_ExpectedBehavior2()
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            string[] changedProperties = new[] { nameof(TEntity.Name) };

            var entity1 = new TEntity();
            var list = new List<TInterface> { entity1 };
            IEnumerable<TInterface> entities = list;
            _MockEntityRepo.Setup(m => m.BulkUpdate(It.Is<PatchedEntityCollection<TInterface, TId>>(pec => pec.PatchedEntities.Count == 1),
                                                    false))
                           .Returns(list);

            // Act
            var result = updateHandler.Update(entities, changedProperties);

            // Assert
            Assert.AreEqual(list, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region UpdateProperty
        [TestMethod]
        public void UpdateHandler_UpdateProperty_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            TId id = 90;
            string property = nameof(TEntity.Name);
            
            string newValue = "Name 90";
            TInterface returnedEntity = new TEntity { Name = newValue };
            _MockEntityRepo.Setup(m => m.Update(It.IsAny<PatchedEntity<TInterface, TId>>(), false))
                           .Returns(returnedEntity);
            // Act
            var result = updateHandler.UpdateProperty(id, property, newValue);

            // Assert
            Assert.AreEqual(newValue, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Replace
        [TestMethod]
        public void UpdateHandler_Replace_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var updateHandler = CreateUpdateHandler();
            TId id = 90;
            var newEntity = new TEntity();
            PatchedEntity<TInterface, TId> patchedEntity = new PatchedEntity<TInterface, TId> { Entity = newEntity };

            TInterface returnedEntity = new TEntity();
            var properties = typeof(TEntity).GetProperties().ToList();
            var propDict = properties.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);
            _MockEntityInfo.Setup(m => m.Properties).Returns(propDict);
            var notValidProperties = new[] { nameof(TEntity.Id), nameof(TEntity.ReadOnlyText) };

            _MockEntityRepo.Setup(m => m.Update(It.Is<PatchedEntity<TInterface, TId>>(pe => !pe.ChangedProperties.Any(cp => notValidProperties.Contains(cp))),
                                                false))
                           .Returns(returnedEntity);


            // Act
            var result = updateHandler.Replace(id, newEntity);

            // Assert
            Assert.AreEqual(result, returnedEntity);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}