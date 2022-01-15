using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TAltKey = System.String;
using TEntity = Rhyous.EntityAnywhere.Services.Common.Tests.AltKeyEntity;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.Services.Common.Tests.IAltKeyEntity;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.Handlers
{
    [TestClass]
    public class SearchByAlternateKeyHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IRepository<TEntity, TInterface, TId>> _MockEntityRepo;
        private Mock<IEntityInfoAltKey<TEntity, TAltKey>> _MockEntityInfoAltKey;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityRepo = _MockRepository.Create<IRepository<TEntity, TInterface, TId>>();
            _MockEntityInfoAltKey = _MockRepository.Create<IEntityInfoAltKey<TEntity, TAltKey>>();
        }

        private SearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> CreateSearchByAlternateKeyHandler()
        {
            return new SearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey>(
                _MockEntityRepo.Object,
                _MockEntityInfoAltKey.Object);
        }

        #region Search
        [TestMethod]
        public void SearchByAlternateKeyHandler_Search_Test()
        {
            // Arrange
            var searchByAlternateKeyHandler = CreateSearchByAlternateKeyHandler();
            TAltKey propertyValue = "PropValue1";
            var entityInfo = new EntityInfoAltKey<TEntity, TAltKey>();
            _MockEntityInfoAltKey.Setup(m => m.PropertyExpression)
                                 .Returns(entityInfo.PropertyExpression);
            IQueryable<TInterface> queryable = new[] { new TEntity() }.AsQueryable();
            _MockEntityRepo.Setup(m => m.Search(propertyValue, entityInfo.PropertyExpression))
                           .Returns(queryable);

            // Act
            var result = searchByAlternateKeyHandler.Search(propertyValue);

            // Assert
            CollectionAssert.AreEqual(queryable.ToList(), result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
