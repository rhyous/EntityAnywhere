using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using System.Collections.Specialized;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GetByIdsHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;
        private Mock<IQueryableHandler<TEntity, TInterface, TId>> _MockQueryableHandler;
        private Mock<IFilterExpressionParser<TEntity>> _MockFilterExpressionParser;
        private Mock<ICustomFilterConvertersRunner<TEntity>> _MockCustomFilterConvertersRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
            _MockQueryableHandler = _MockRepository.Create<IQueryableHandler<TEntity, TInterface, TId>>();
            _MockFilterExpressionParser = _MockRepository.Create<IFilterExpressionParser<TEntity>>();
            _MockCustomFilterConvertersRunner = _MockRepository.Create<ICustomFilterConvertersRunner<TEntity>>();
        }

        private GetByIdsHandler<TEntity, TInterface, TId> CreateGetByIdsHandler()
        {
            return new GetByIdsHandler<TEntity, TInterface, TId>(
                _MockEntityRepo.Object,
                _MockQueryableHandler.Object,
                _MockFilterExpressionParser.Object,
                _MockCustomFilterConvertersRunner.Object);
        }

        #region GetAsync
        [TestMethod]
        public async Task GetByIdsHandler_GetAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var getByIdsHandler = CreateGetByIdsHandler();
            IEnumerable<int> ids = null;
            NameValueCollection parameters = null;
            IQueryable<TInterface> queryable = new List<TInterface>().AsQueryable();
            _MockEntityRepo.Setup(m => m.Get(ids)).Returns(queryable);
            _MockQueryableHandler.Setup(m => m.GetQueryable(queryable, null, null, -1, -1))
                                 .Returns(queryable);

            // Act
            var result = await getByIdsHandler.GetAsync(ids, parameters);

            // Assert
            Assert.AreEqual(queryable, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
