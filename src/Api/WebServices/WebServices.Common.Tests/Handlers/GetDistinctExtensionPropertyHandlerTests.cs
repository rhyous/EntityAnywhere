using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System.Linq.Expressions;
using System;
using LinqKit;
using System.Collections.Generic;
using Rhyous.EntityAnywhere.Entities;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.ExtensionEntityBasic;
using TId = System.Int64;
using TInterface = Rhyous.EntityAnywhere.Interfaces.IExtensionEntity;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetDistinctExtensionPropertyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
        }

        private GetDistinctPropertyValuesHandler<TEntity, TInterface, TId> CreateGetDistinctExtensionPropertyHandler()
        {
            return new GetDistinctPropertyValuesHandler<TEntity, TInterface, TId>(
                _MockServiceCommon.Object);
        }

        #region Handle
        [TestMethod]
        public void GetDistinctExtensionPropertyHandler_Handle_With_PreExpression()
        {
            // Arrange
            var getDistinctExtensionPropertyHandler = CreateGetDistinctExtensionPropertyHandler();
            string entity = "User";
            string property = "Property";

            var addenda1 = new Addendum() { Id = 1, EntityId = "100", Entity = "User", Property = "TestProp", Value = "TestValue" };
            var addenda2 = new Addendum() { Id = 2, EntityId = "101", Entity = "User", Property = "TestProp", Value = "TestValue2" };
            var addenda3 = new Addendum() { Id = 3, EntityId = "500", Entity = "Feature", Property = "FTestProp", Value = "TestValue3" };
            var returnList = new List<object> { addenda1, addenda2 };

            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Entity == entity);
            _MockServiceCommon.Setup(m => m.GetDistinctPropertyValues(It.IsAny<string>(), It.IsAny<Expression<Func<TEntity, bool>>>()))
                              .Returns(returnList);

            // Act
            var result = getDistinctExtensionPropertyHandler.Handle(property, expression);

            // Assert
            Assert.AreEqual(result.Count, 2);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetDistinctExtensionPropertyHandler_Handle_No_PreExpression()
        {
            // Arrange
            var getDistinctExtensionPropertyHandler = CreateGetDistinctExtensionPropertyHandler();
            string entity = "User";
            string property = "Property";

            var addenda1 = new Addendum() { Id = 1, EntityId = "100", Entity = "User", Property = "TestProp", Value = "TestValue" };
            var addenda2 = new Addendum() { Id = 2, EntityId = "101", Entity = "User", Property = "TestProp", Value = "TestValue2" };
            var addenda3 = new Addendum() { Id = 3, EntityId = "500", Entity = "Feature", Property = "FTestProp", Value = "TestValue3" };
            var returnList = new List<object> { addenda1, addenda2, addenda3 };

            Expression<Func<TEntity, bool>> expression = PredicateBuilder.New<TEntity>(e => e.Entity == entity);
            _MockServiceCommon.Setup(m => m.GetDistinctPropertyValues(It.IsAny<string>(), It.IsAny<Expression<Func<TEntity, bool>>>()))
                              .Returns(returnList);

            // Act
            var result = getDistinctExtensionPropertyHandler.Handle(property, null);

            // Assert
            Assert.AreEqual(result.Count, 3);
            _MockRepository.VerifyAll();
        }

        #endregion
    }
}
