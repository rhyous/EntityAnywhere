using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GenerateRepositoryHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
        }

        private GenerateRepositoryHandler<TEntity, TInterface, TId> CreateGenerateRepositoryHandler()
        {
            return new GenerateRepositoryHandler<TEntity, TInterface, TId>(_MockEntityRepo.Object);
        }

        #region GenerateRepository
        [TestMethod]
        public void GenerateRepositoryHandler_GenerateRepository_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var generateRepositoryHandler = CreateGenerateRepositoryHandler();
            var repoGenResult = new RepositoryGenerationResult
            {
                Name = typeof(TEntity).Name,
                RepositoryReady = true
            };
            _MockEntityRepo.Setup(m => m.GenerateRepository()).Returns(repoGenResult);

            // Act
            var result = generateRepositoryHandler.GenerateRepository();

            // Assert
            Assert.AreEqual(repoGenResult, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
