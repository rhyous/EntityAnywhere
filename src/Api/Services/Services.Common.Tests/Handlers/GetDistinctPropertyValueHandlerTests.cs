using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class GetDistinctPropertyValueHandlerTests
    {
        private MockRepository _MockRepository;
        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
        }

        private GetDistinctPropertyValueHandler<TEntity, TInterface, TId> CreateGetDistinctPropertyValueHandler()
        {
            return new GetDistinctPropertyValueHandler<TEntity, TInterface, TId>(_MockEntityRepo.Object);
        }

        #region Get
        [TestMethod]
        public void GetDistinctPropertyValueHandler_Get_Returns_EntityList()
        {
            // Arrange
            var getDistinctPropertyValueHandler = CreateGetDistinctPropertyValueHandler();
            string propertyName = "Name";
            Expression<Func<TEntity, bool>> preExpression = null;

            TInterface entity = new TEntity { Name = "abc name" };
            TInterface entity2 = new TEntity { Name = "xyz name" };
            List<object> entitylist = new List<object> { entity, entity2 };
            _MockEntityRepo.Setup(m => m.GetDistinctPropertyValues(It.IsAny<string>(), preExpression))
                           .Returns(entitylist);

            // Act
            var result = getDistinctPropertyValueHandler.Get(propertyName, preExpression);

            // Assert
            Assert.IsNotNull(result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
