using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using TAltKey = System.String;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.AltKeyEntity;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IAltKeyEntity;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GetByAlternateKeyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;
        private Mock<IEntityInfoAltKey<TEntity, TAltKey>> _MockEntityInfoAltKey;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
            _MockEntityInfoAltKey = _MockRepository.Create<IEntityInfoAltKey<TEntity, TAltKey>>();
        }

        private GetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> CreateGetByAlternateKeyHandler()
        {
            return new GetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>(
                _MockEntityRepo.Object,
                _MockEntityInfoAltKey.Object);
        }

        #region Get
        [TestMethod]
        public void GetByAlternateKeyHandler_Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var getByAlternateKeyHandler = CreateGetByAlternateKeyHandler();
            TAltKey propertyValue = default(TAltKey);
            var entityInfo = new EntityInfoAltKey<TEntity, TAltKey>();
            _MockEntityInfoAltKey.Setup(m=>m.PropertyExpression)
                                 .Returns(entityInfo.PropertyExpression);
            TInterface entity = new TEntity();
            _MockEntityRepo.Setup(m => m.Get(propertyValue, entityInfo.PropertyExpression))
                           .Returns(entity);

            // Act
            var result = getByAlternateKeyHandler.Get(propertyValue);

            // Assert
            Assert.AreEqual(entity, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
