using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GetPropertyValueHandlerTests
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

        private GetSelectPropertiesHandler<TEntity, TInterface, TId> CreateGetPropertyValueHandler()
        {
            return new GetSelectPropertiesHandler<TEntity, TInterface, TId>(
                _MockEntityRepo.Object,
                _MockEntityInfo.Object);
        }

        #region GetProperty
        [TestMethod]
        public void GetPropertyValueHandler_GetProperty_Test()
        {
            // Arrange
            var getPropertyValueHandler = CreateGetPropertyValueHandler();
            TId id = 1027;
            string property = nameof(TEntity.Name);
            EntityInfo<TEntity> entityInfo = new EntityInfo<TEntity>();
            _MockEntityInfo.Setup(m => m.Properties).Returns(entityInfo.Properties);
            TInterface entity = new TEntity { Name = "abc name"};
            _MockEntityRepo.Setup(m => m.GetSelectProperties(id, property)).Returns(entity);

            // Act
            var result = getPropertyValueHandler.GetSelectProperty(id, property);

            // Assert
            Assert.AreEqual(entity.Name, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
