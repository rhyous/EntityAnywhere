using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.AltKeyEntity;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IAltKeyEntity;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GetByIdHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
        }

        private GetByIdHandler<TEntity, TInterface, TId> CreateGetByIdHandler()
        {
            return new GetByIdHandler<TEntity, TInterface, TId>(_MockEntityRepo.Object);
        }

        #region Get
        [TestMethod]
        public void GetByIdHandler_Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var getByIdHandler = CreateGetByIdHandler();
            TId id = 1027;
            TInterface entity = new TEntity();
            _MockEntityRepo.Setup(m => m.Get(id)).Returns(entity);

            // Act
            var result = getByIdHandler.Get(id);

            // Assert
            Assert.AreEqual(entity, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
