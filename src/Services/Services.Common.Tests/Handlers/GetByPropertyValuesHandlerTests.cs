using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Linq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.UnitTesting;
using LinqKit;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GetByPropertyValuesHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IQueryableHandler<TEntity, TInterface, TId>> _MockQueryableHandler;
        private Mock<IFilterExpressionParser<TEntity>> _MockFilterExpressionParser;
        private Mock<ICustomFilterConvertersRunner<TEntity>> _MockCustomFilterConvertersRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockQueryableHandler = _MockRepository.Create<IQueryableHandler<TEntity, TInterface, TId>>();
            _MockFilterExpressionParser = _MockRepository.Create<IFilterExpressionParser<TEntity>>();
            _MockCustomFilterConvertersRunner = _MockRepository.Create<ICustomFilterConvertersRunner<TEntity>>();
        }

        private GetByPropertyValuesHandler<TEntity, TInterface, TId> CreateGetByPropertyValuesHandler()
        {
            return new GetByPropertyValuesHandler<TEntity, TInterface, TId>(
                _MockQueryableHandler.Object,
                _MockFilterExpressionParser.Object,
                _MockCustomFilterConvertersRunner.Object);
        }

        #region GetAsync
        [TestMethod]
        public async Task GetByPropertyValuesHandler_GetAsync_String_Test()
        {
            // Arrange
            var getByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string property = nameof(TEntity.Name);
            IEnumerable<string> values = new[] { "Name1", "Name2" };
            NameValueCollection parameters = null;
            var expressionString = "e => value(System.Collections.Generic.List`1[System.String]).Contains(e.Name)";
            IQueryable<TInterface> queryable = new[] { new TEntity() }.AsQueryable();
            _MockQueryableHandler.Setup(m => m.GetQueryable(It.Is<Expression<Func<TEntity, bool>>>(e => e.ToString() == expressionString),
                                                            -1, -1, "Id", SortOrder.Ascending))
                                 .Returns(queryable);


            // Act
            var result = await getByPropertyValuesHandler.GetAsync(
                property,
                values,
                parameters);

            // Assert
            Assert.AreEqual(queryable, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GetAsync
        [TestMethod]
        [ObjectNullOrNew(typeof(NameValueCollection))]
        public async Task GetByPropertyValuesHandler_GetAsync_LongProperty_NullOrNewNameValueCollection_Test(NameValueCollection nvc)
        {
            // Arrange
            var getByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string property = nameof(TEntity.LongNumber);
            IEnumerable<long> values = new[] { 0L, long.MaxValue };
            NameValueCollection parameters = nvc;
            var expressionString = "e => value(System.Collections.Generic.List`1[System.Int64]).Contains(e.LongNumber)";
            IQueryable<TInterface> queryable = new[] { new TEntity() }.AsQueryable();
            _MockQueryableHandler.Setup(m => m.GetQueryable(It.Is<Expression<Func<TEntity, bool>>>(e => e.ToString() == expressionString),
                                                            -1, -1, "Id", SortOrder.Ascending))
                                 .Returns(queryable);

            // Act
            var result = await getByPropertyValuesHandler.GetAsync(
                property,
                values,
                parameters);

            // Assert
            Assert.AreEqual(queryable, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByPropertyValuesHandler_GetAsync_LongProperty_NameValueCollectionHasNoFilter_Test()
        {
            // Arrange
            var getByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string property = nameof(TEntity.LongNumber);
            IEnumerable<long> values = new[] { 0L, long.MaxValue };
            NameValueCollection parameters = new NameValueCollection { { "PropA", "ValueA" } };
            var expressionString = "e => value(System.Collections.Generic.List`1[System.Int64]).Contains(e.LongNumber)";
            IQueryable<TInterface> queryable = new[] { new TEntity() }.AsQueryable();
            _MockQueryableHandler.Setup(m => m.GetQueryable(It.Is<Expression<Func<TEntity, bool>>>(e => e.ToString() == expressionString),
                                                            -1, -1, "Id", SortOrder.Ascending))
                                 .Returns(queryable);

            // Act
            var result = await getByPropertyValuesHandler.GetAsync(
                property,
                values,
                parameters);

            // Assert
            Assert.AreEqual(queryable, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByPropertyValuesHandler_GetAsync_LongProperty_NameValueCollectionHasFilter_ReturnsNullExpression_Test()
        {
            // Arrange
            var getByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string property = nameof(TEntity.LongNumber);
            IEnumerable<long> values = new[] { 0L, long.MaxValue };
            NameValueCollection parameters = new NameValueCollection { { "$Filter", "Contains(Name, A)" } };
            Expression<Func<TEntity, bool>> expression = null;
            _MockFilterExpressionParser.Setup(m => m.ParseAsync(parameters, false, _MockCustomFilterConvertersRunner.Object))
                                       .ReturnsAsync(expression);

            var expressionString = "e => value(System.Collections.Generic.List`1[System.Int64]).Contains(e.LongNumber)";
            IQueryable<TInterface> queryable = new[] { new TEntity() }.AsQueryable();
            _MockQueryableHandler.Setup(m => m.GetQueryable(It.Is<Expression<Func<TEntity, bool>>>(e => e.ToString() == expressionString),
                                                            -1, -1, "Id", SortOrder.Ascending))
                                 .Returns(queryable);

            // Act
            var result = await getByPropertyValuesHandler.GetAsync(
                property,
                values,
                parameters);

            // Assert
            Assert.AreEqual(queryable, result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByPropertyValuesHandler_GetAsync_LongProperty_NameValueCollectionHasFilter_ReturnsValidExpression_Test()
        {
            // Arrange
            var getByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string property = nameof(TEntity.LongNumber);
            IEnumerable<long> values = new[] { 0L, long.MaxValue };
            NameValueCollection parameters = new NameValueCollection { { "$Filter", "Contains(Name, A)" } };

            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Name.Contains("A"));
            _MockFilterExpressionParser.Setup(m => m.ParseAsync(parameters, false, _MockCustomFilterConvertersRunner.Object))
                                       .ReturnsAsync(expression);

            var expressionString = "e => (value(System.Collections.Generic.List`1[System.Int64]).Contains(e.LongNumber) AndAlso e.Name.Contains(\"A\"))";
            IQueryable<TInterface> queryable = new[] { new TEntity() }.AsQueryable();
            _MockQueryableHandler.Setup(m => m.GetQueryable(It.Is<Expression<Func<TEntity, bool>>>(e => e.ToString() == expressionString),
                                                            -1, -1, "Id", SortOrder.Ascending))
                                 .Returns(queryable);

            // Act
            var result = await getByPropertyValuesHandler.GetAsync(
                property,
                values,
                parameters);

            // Assert
            Assert.AreEqual(queryable, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}