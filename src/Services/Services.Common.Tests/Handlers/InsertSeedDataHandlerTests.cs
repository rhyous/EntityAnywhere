using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.AltKeyEntity;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IAltKeyEntity;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class InsertSeedDataHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
        }

        private InsertSeedDataHandler<TEntity, TInterface, TId> CreateInsertSeedDataHandler()
        {
            return new InsertSeedDataHandler<TEntity, TInterface, TId>(_MockEntityRepo.Object);
        }

        #region InsertSeedData
        [TestMethod]
        public void InsertSeedDataHandler_InsertSeedData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var insertSeedDataHandler = CreateInsertSeedDataHandler();

            // Act
            var result = insertSeedDataHandler.InsertSeedData();

            // Assert
            Assert.IsFalse(result.EntityHasSeedData);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
