using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityBasic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class DeleteHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityEventAll<TEntity, TId>> _MockEntityEventAll;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityEventAll = _MockRepository.Create<IEntityEventAll<TEntity, TId>>();
            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private DeleteHandler<TEntity, TInterface, TId> CreateDeleteHandler()
        {
            return new DeleteHandler<TEntity, TInterface, TId>(
                _MockEntityEventAll.Object,
                _MockServiceCommon.Object);
        }

        [TestMethod]
        public void DeleteHandler_Handle_MethodsWereCalled_Test()
        {
            // Arrange
            var deleteHandler = CreateDeleteHandler();
            int id = 27;
            var entity = new TEntity { Id = id };
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<int>())).Returns(entity);
            _MockEntityEventAll.Setup(m => m.BeforeDelete(It.IsAny<TEntity>()));
            _MockServiceCommon.Setup(m => m.Delete(It.IsAny<TId>())).Returns(true);
            _MockEntityEventAll.Setup(m => m.AfterDelete(It.IsAny<TEntity>(), It.IsAny<bool>()));

            // Act
            var result = deleteHandler.Handle(id.ToString());

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteHandler_Handle_AlreadyDeleted_MethodsNotCalled_Test()
        {
            // Arrange
            var deleteHandler = CreateDeleteHandler();
            int id = 27;
            TEntity entity = null;
            _MockServiceCommon.Setup(m => m.Get(It.IsAny<int>())).Returns(entity);

            // Act
            var result = deleteHandler.Handle(id.ToString());

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }
    }
}