using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class AddAltKeyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityInfoAltKey<AltKeyEntity, string>> _MockEntityInfoAltKey;
        private Mock<IGetByPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>> _MockGetByPropertyValuesHandler;
        private Mock<IAddHandler<AltKeyEntity, IAltKeyEntity, int>> _MockAddHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityInfoAltKey = _MockRepository.Create<IEntityInfoAltKey<AltKeyEntity, string>>();
            _MockGetByPropertyValuesHandler = _MockRepository.Create<IGetByPropertyValuesHandler<AltKeyEntity, IAltKeyEntity, int>>();
            _MockAddHandler = _MockRepository.Create<IAddHandler<AltKeyEntity, IAltKeyEntity, int>>();
        }

        private AddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string> CreateAddAltKeyHandler()
        {
            return new AddAltKeyHandler<AltKeyEntity, IAltKeyEntity, int, string>(
                _MockEntityInfoAltKey.Object,
                _MockGetByPropertyValuesHandler.Object,
                _MockAddHandler.Object);
        }

        #region Add
        [TestMethod]
        public async Task AddAltKeyHandler_Add_Duplicate_ThrowsException_Test()
        {
            // Arrange
            var service = CreateAddAltKeyHandler();
            var entity = new AltKeyEntity { Id = 101, Name = "Duplicate1" };

            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);
            _MockEntityInfoAltKey.Setup(m => m.AlternateKeyProperty)
                                 .Returns(entityInfoAltKey.AlternateKeyProperty);

            _MockGetByPropertyValuesHandler.Setup(m => m.GetAsync<string>(nameof(AltKeyEntity.Name), 
                                                                          It.Is<IEnumerable<string>>(e => e.First() == entity.Name),
                                                                          null))
                                           .ReturnsAsync(new[] { entity}.AsQueryable());


            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<DuplicateKeyException>(async () =>
            {
                await service.AddAsync(new[] { entity });
            });
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [ArrayNullOrEmpty(typeof(AltKeyEntity))]
        public async Task AddAltKeyHandler_Add_NotDuplicate_Test(AltKeyEntity[] foundAltIdEntities)
        {
            // Arrange
            var service = CreateAddAltKeyHandler();
            var entity = new AltKeyEntity { Id = 101, Name = "Name1" };

            var entityInfoAltKey = new EntityInfoAltKey<AltKeyEntity, string>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpressionMethod)
                                 .Returns(entityInfoAltKey.PropertyExpressionMethod);
            _MockEntityInfoAltKey.Setup(m => m.AlternateKeyProperty)
                                 .Returns(entityInfoAltKey.AlternateKeyProperty);

            _MockGetByPropertyValuesHandler.Setup(m => m.GetAsync<string>(nameof(AltKeyEntity.Name),
                                                                          It.Is<IEnumerable<string>>(e => e.First() == entity.Name),
                                                                          null))
                                           .ReturnsAsync(foundAltIdEntities?.AsQueryable());

            _MockAddHandler.Setup(m => m.AddAsync(It.IsAny<IEnumerable<AltKeyEntity>>()))
                           .ReturnsAsync(new List<IAltKeyEntity> { entity });

            // Act
            var actual = await service.AddAsync(new[] { entity });

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
