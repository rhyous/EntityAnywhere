using LinqKit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Filter;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;
using System.IO;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class QueryableHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockRepo;
        private Mock<IEntitySettingsCache> _MockEntitySettingsCache;
        private Mock<IFilterExpressionParser<TEntity>> _MockFilterExpressionParser;
        private Mock<ICustomFilterConvertersRunner<TEntity>> _MockCustomFilterConvertersRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
            _MockEntitySettingsCache = _MockRepository.Create<IEntitySettingsCache>();
            _MockFilterExpressionParser = _MockRepository.Create<IFilterExpressionParser<TEntity>>();
            _MockCustomFilterConvertersRunner = _MockRepository.Create<ICustomFilterConvertersRunner<TEntity>>();
        }

        private QueryableHandler<TEntity, TInterface, TId> CreateQueryableHandler()
        {
            return new QueryableHandler<TEntity, TInterface, TId>(
                _MockRepo.Object,
                _MockEntitySettingsCache.Object,
                _MockFilterExpressionParser.Object,
                _MockCustomFilterConvertersRunner.Object);
        }

        #region GetQueryable
        [TestMethod]
        public void QueryableHandler_GetQueryable_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            IQueryable<TInterface> queryable = null;
            var entitySettings = new EntitySettings
            {
                SortByProperty = nameof(TEntity.Name),
                Entity = new Entity { SortOrder = SortOrder.Descending }
            };
            _MockEntitySettingsCache.Setup(m => m.Provide(typeof(TEntity)))
                     .Returns(entitySettings);
            _MockRepo.Setup(m => m.Get(entitySettings.SortByProperty, entitySettings.Entity.SortOrder))
                     .Returns(queryable);

            // Act
            var result = queryableHandler.GetQueryable();

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetQueryable
        [TestMethod]
        [JsonTestDataSource(typeof(List<TestDataRow<string>>), @"Data/ComplexFilterStrings.json")]
        public async Task QueryableHandler_Get_TwoQuotesToOneQuote_Json(TestDataRow<string> row)
        {
            // Arrange
            var service = CreateQueryableHandler();

            var query = row.TestValue;
            var expected = row.Expected;
            var msg = row.Message;
            var collection = new NameValueCollection { { "$filter", query } };
            var mockLogger = new Mock<ILogger>();
            var entitySettings = new EntitySettings
            {
                SortByProperty = nameof(TEntity.Name),
                Entity = new Entity { SortOrder = SortOrder.Descending }
            };
            _MockEntitySettingsCache.Setup(m => m.Provide(typeof(TEntity)))
                     .Returns(entitySettings);
            var realFilterExpressionParser = new FilterExpressionParser<TEntity>(FilterExpressionParserActionDictionary<TEntity>.Instance);
            _MockFilterExpressionParser.Setup(m => m.ParseAsync(collection, true, _MockCustomFilterConvertersRunner.Object))
                                       .ReturnsAsync((NameValueCollection parameters, bool unquote, ICustomFilterConvertersRunner<TEntity> customFilterConverter) =>
                                                {
                                                    return realFilterExpressionParser.ParseAsync(parameters, unquote, customFilterConverter).Result;
                                                });
            _MockCustomFilterConvertersRunner.Setup(m => m.ConvertAsync(It.IsAny<Filter<TEntity>>()))
                                             .ReturnsAsync((Filter<TEntity> f) => f);

            string inputQuery = null;
            _MockRepo.Setup(m => m.GetByExpression(It.IsAny<Expression<Func<TEntity, bool>>>(),
                                                        It.IsAny<string>(),
                                                        It.IsAny<SortOrder>()))
                          .Returns((Expression<Func<TEntity, bool>> expression,
                                    string sortByProperty,
                                    SortOrder sortOrder) =>
                          {
                              inputQuery = expression.ToString();
                              return (null);
                          });

            // Act
            var actual = await service.GetQueryableAsync(collection);

            // Assert
            Assert.AreEqual(expected, inputQuery);
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetQueryable Expression
        [TestMethod]
        public void QueryableHandler_GetQueryable_Expression_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Id == 27);
            int take = 0;
            int skip = 0;
            string sortProperty = "SortProp";
            SortOrder sortOrder = SortOrder.Descending;
            IQueryable<TInterface> queryable = null;
            _MockRepo.Setup(m => m.GetByExpression(expression, sortProperty, sortOrder))
                     .Returns(queryable);

            // Act
            var result = queryableHandler.GetQueryable(
                expression,
                take,
                skip,
                sortProperty,
                sortOrder);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetQueryableWithModifier
        [TestMethod]
        public void QueryableHandler_GetQueryableWithModifier_Null_ReturnedQueryable_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier = null;
            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Id == 27);
            string sortProperty = "Id";
            SortOrder sortOrder = SortOrder.Ascending;
            IQueryable<TInterface> queryable = null;
            _MockRepo.Setup(m => m.GetByExpression(expression, sortProperty, sortOrder))
                     .Returns(queryable);

            // Act
            var result = queryableHandler.GetQueryableWithModifier(
                queryableModifier,
                expression);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void QueryableHandler_GetQueryableWithModifier_Null_Modifier_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier = null;
            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Id == 27);
            string sortProperty = "Id";
            SortOrder sortOrder = SortOrder.Ascending;
            IQueryable<TInterface> queryable = new List<TInterface>().AsQueryable();
            _MockRepo.Setup(m => m.GetByExpression(expression, sortProperty, sortOrder))
                     .Returns(queryable);

            // Act
            var result = queryableHandler.GetQueryableWithModifier(
                queryableModifier,
                expression);

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void QueryableHandler_GetQueryableWithModifier_Valid_Modifier_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            bool queryableWasCalled = false;
            Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier = (IQueryable<TInterface> q) => 
            {
                queryableWasCalled = true;
                return q.ToList(); 
            };
            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Id == 27);
            string sortProperty = "Id";
            SortOrder sortOrder = SortOrder.Ascending;
            IQueryable<TInterface> queryable = new List<TInterface>().AsQueryable();
            _MockRepo.Setup(m => m.GetByExpression(expression, sortProperty, sortOrder))
                     .Returns(queryable);

            // Act
            var result = queryableHandler.GetQueryableWithModifier(
                queryableModifier,
                expression);

            // Assert
            Assert.IsTrue(queryableWasCalled);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetQueryable queryable expression
        [TestMethod]
        public void QueryableHandler_GetQueryable_Queryable_And_Expression_Null_Querable_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            IQueryable<TEntity> queryable = null;
            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Id == 27);
            Expression<Func<TEntity, TId>> orderExpression = e => e.Id;
            int take = 0;
            int skip = 0;

            // Act
            var result = queryableHandler.GetQueryable(
                queryable,
                expression,
                orderExpression,
                take,
                skip);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void QueryableHandler_GetQueryable_Queryable_And_Expression_Valid_Querable_ForwardsToRepo_Test()
        {
            // Arrange
            var queryableHandler = CreateQueryableHandler();
            IQueryable<TEntity> queryable = new List<TEntity>().AsQueryable();
            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Id == 27);
            Expression<Func<TEntity, TId>> orderExpression = e => e.Id;
            int take = 0;
            int skip = 0;

            // Act
            var result = queryableHandler.GetQueryable(
                queryable,
                expression,
                orderExpression,
                take,
                skip);

            // Assert
            Assert.IsNotNull(result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
