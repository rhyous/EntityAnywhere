using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class UpdateAltKeyHandlerTests
    {
        private MockRepository _MockRepository;

        private EntityInfoAltKey<AltKeyEntity, string> _EntityInfoAltKey;
        private Mock<IGetByPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>> _MockGetByPropertyValuesHandler;
        private Mock<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>> _MockGetByAlternateKeyHandler;
        private Mock<IUpdateHandler<AltKeyEntity, IAltKeyEntity, int>> _MockUpdateHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _EntityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockGetByPropertyValuesHandler = _MockRepository.Create<IGetByPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>>();
            _MockGetByAlternateKeyHandler = _MockRepository.Create<IGetByAlternateKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>>();
            _MockUpdateHandler = _MockRepository.Create<IUpdateHandler<AltKeyEntity, IAltKeyEntity, int>>();
        }

        private UpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string> CreateUpdateAltKeyHandler()
        {
            return new UpdateAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string> (
                _EntityInfoAltKey,
                _MockGetByPropertyValuesHandler.Object,
                _MockGetByAlternateKeyHandler.Object,
                _MockUpdateHandler.Object);
        }

        #region Update
        [TestMethod]
        public void UpdateAltKeyHandler_Update_ToOtherNonDuplicateValue_Test()
        {
            // Arrange
            var service = CreateUpdateAltKeyHandler();
            var entity = new AltKeyEntity { Id = 101, Name = "Name1" };
            var entityUpdate = new AltKeyEntity { Id = 101, Name = "Name2" };


            _MockGetByAlternateKeyHandler.Setup(m => m.Get(entityUpdate.Name))
                                         .Returns((IAltKeyEntity)null);

            var patchedEntity = new PatchedEntity<IAltKeyEntity, int>
            {
                Entity = entityUpdate,
                ChangedProperties = new HashSet<string> { "Name" }
            };
            _MockUpdateHandler.Setup(m => m.Update(entityUpdate.Id, patchedEntity))
                              .Returns(entityUpdate);

            // Act
            var actual = service.Update(101, patchedEntity);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateAltKeyHandler_Update_ToSameValue_Test()
        {
            // Arrange
            var service = CreateUpdateAltKeyHandler();
            var entity = new AltKeyEntity { Id = 101, Name = "Name1" };
            var entityUpdate = new AltKeyEntity { Id = 101, Name = "Name1" };


            _MockGetByAlternateKeyHandler.Setup(m => m.Get(entityUpdate.Name))
                                         .Returns((IAltKeyEntity)null);

            var patchedEntity = new PatchedEntity<IAltKeyEntity, int>
            {
                Entity = entityUpdate,
                ChangedProperties = new HashSet<string> { "Name" }
            };
            _MockUpdateHandler.Setup(m => m.Update(entityUpdate.Id, patchedEntity))
                              .Returns(entityUpdate);

            // Act
            var actual = service.Update(101, patchedEntity);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void UpdateAltKeyHandler_Update_AnyOtherField_Test()
        {
            // Arrange
            var service = CreateUpdateAltKeyHandler();
            var entity = new AltKeyEntity { Id = 101, Name = "Name1", Description = "A sample entity." };
            var entityUpdate = new AltKeyEntity { Id = 101, Name = "Name1", Description = "An updated sample entity." };

            var patchedEntity = new PatchedEntity<IAltKeyEntity, int>
            {
                Entity = entityUpdate,
                ChangedProperties = new HashSet<string> { "Description" }
            };
            _MockUpdateHandler.Setup(m => m.Update(entityUpdate.Id, patchedEntity))
                              .Returns(entityUpdate);

            // Act
            var actual = service.Update(101, patchedEntity);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion        
    }
}