using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityBasic;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityBasic;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetAllHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityFetcher;
        private NameValueCollection _NameValueCollection;
        private IRequestUri _RequestUri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _MockRelatedEntityFetcher = _MockRepository.Create<IRelatedEntityProvider<TEntity, TInterface, TId>>();
            _NameValueCollection = new NameValueCollection { };
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetAllHandler<TEntity, TInterface, TId> CreateGetAllHandler()
        {
            return new GetAllHandler<TEntity, TInterface, TId>(
                _MockServiceCommon.Object,
                _MockRelatedEntityFetcher.Object,
                new UrlParameters { Collection = _NameValueCollection },
                _RequestUri);
        }

        [TestMethod]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var queryable1 = new List<TInterface> { new TEntity(), new TEntity() };
            var queryable2 = new List<TInterface>(queryable1) { new TEntity(), new TEntity() };
            _MockServiceCommon.SetupSequence(m => m.GetAsync(It.IsAny<NameValueCollection>()))
                             .ReturnsAsync(queryable1.AsQueryable())
                             .ReturnsAsync(queryable2.AsQueryable());
            var getAllHandler = CreateGetAllHandler();

            // Act
            var result = await getAllHandler.HandleAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(4, result.TotalCount);
            _MockServiceCommon.Verify(m => m.GetAsync(It.IsAny<NameValueCollection>()), Times.Exactly(2));
        }
    }
}