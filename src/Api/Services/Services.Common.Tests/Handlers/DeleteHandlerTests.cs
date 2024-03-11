using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class DeleteHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
        }

        private DeleteHandler<TEntity, TInterface, TId> CreateDeleteHandler()
        {
            return new DeleteHandler<TEntity, TInterface, TId>(_MockEntityRepo.Object);
        }

        #region Delete
        [TestMethod]
        public void DeleteHandler_Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var deleteHandler = CreateDeleteHandler();
            TId id = 1027;
            _MockEntityRepo.Setup(m=>m.Delete(id)).Returns(true);

            // Act
            var result = deleteHandler.Delete(id);

            // Assert
            Assert.IsTrue(result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region DeleteMany
        [TestMethod]
        public void DeleteHandler_DeleteMany_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var deleteHandler = CreateDeleteHandler();
            IEnumerable<TId> ids = new List<TId> { 11, 12, 13 };
            var resultDictionary = new Dictionary<TId, bool> { { 11, true }, { 12, true }, { 13, true } };
            _MockEntityRepo.Setup(m => m.DeleteMany(ids)).Returns(resultDictionary);

            // Act
            var result = deleteHandler.DeleteMany(ids);

            // Assert
            Assert.AreEqual(resultDictionary, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
