using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;


namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class AddHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
        }

        private AddHandler<TEntity, TInterface, TId> CreateAddHandler()
        {
            return new AddHandler<TEntity, TInterface, TId>(_MockEntityRepo.Object);
        }

        #region AddAsync
        [TestMethod]
        public async Task AddHandler_AddAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var addHandler = CreateAddHandler();
            IEnumerable<TInterface> entities = null;
            var list = new List<TInterface>();
            _MockEntityRepo.Setup(m=>m.Create(entities)).Returns(list);

            // Act
            var result = await addHandler.AddAsync(entities);

            // Assert
            Assert.AreEqual(list, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
