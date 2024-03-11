using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class AddAltKeyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IAddHandler<AltKeyEntity, IAltKeyEntity, int>> _MockAddHandler;
        private Mock<IDuplicateEntityPreventer<AltKeyEntity, IAltKeyEntity, int, string>> _MockDuplicateEntityPreventer;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockAddHandler = _MockRepository.Create<IAddHandler<AltKeyEntity, IAltKeyEntity, int>> ();
            _MockDuplicateEntityPreventer = _MockRepository.Create<IDuplicateEntityPreventer<AltKeyEntity, IAltKeyEntity, int, string>>();
        }

        private AddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string> CreateAddAltKeyHandler()
        {
            return new AddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>(
                _MockAddHandler.Object,
                _MockDuplicateEntityPreventer.Object);
        }

        #region Add
        [TestMethod]
        public async Task AddAltKeyHandler_Add_NotDuplicate_Test()
        {
            // Arrange
            var service = CreateAddAltKeyHandler();
            var entity = new AltKeyEntity { Id = 101, Name = "Name1" };
            var entities = new[] { entity };
            _MockDuplicateEntityPreventer.Setup(m => m.CheckAsync(entities))
                                         .Returns(Task.CompletedTask);
            _MockAddHandler.Setup(m => m.AddAsync(It.IsAny<IEnumerable<AltKeyEntity>>()))
                           .ReturnsAsync(new List<IAltKeyEntity> { entity });
            _MockDuplicateEntityPreventer.Setup(m => m.RemoveTracked(entities));

            // Act
            var actual = await service.AddAsync(entities);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
