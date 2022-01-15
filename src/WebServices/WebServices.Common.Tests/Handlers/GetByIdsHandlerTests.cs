using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    [TestClass]
    public class GetByIdsHandlerTests
    {
        private MockRepository _MockRepository;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private IUrlParameters _IUrlParameters;
        private IRequestUri _RequestUri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<TEntity, TInterface, TId>>();
            _IUrlParameters = new UrlParameters { Collection = new NameValueCollection() };
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics(101)") };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task GetByIdsHandler_Handle_NullList_Test()
        {
            // Arrange
            List<int> ids = null;
            var getHandler = new GetByIdsHandler<EntityInt, IEntityInt, int>(_MockServiceCommon.Object,
                                                                             _MockRelatedEntityProvider.Object,
                                                                             _IUrlParameters,
                                                                             _RequestUri);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await getHandler.HandleAsync(ids);
            });
            _MockServiceCommon.Verify(m => m.Get(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetByIdsHandler_Handle_EmptyList_Test()
        {
            // Arrange
            var ids = new List<int>();
            var mockServiceCommon = new Mock<ServiceCommon<EntityInt, IEntityInt, int>>();
            var getHandler = new GetByIdsHandler<EntityInt, IEntityInt, int>(_MockServiceCommon.Object,
                                                                             _MockRelatedEntityProvider.Object,
                                                                             _IUrlParameters, _RequestUri);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await getHandler.HandleAsync(ids);
            });
            mockServiceCommon.Verify(m => m.Get(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task GetByIdsHandler_Handle_ValidList_Test()
        {
            // Arrange
            var ids = new List<int> { 27, 128 };
            var entityInts = new List<IEntityInt>
            {
                new EntityInt { Id = 27 },
                new EntityInt { Id = 128 }
            };
            _MockServiceCommon.Setup(m => m.GetAsync(It.IsAny<IEnumerable<int>>(), It.IsAny<NameValueCollection>()))
                             .ReturnsAsync(entityInts.AsQueryable());
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<EntityInt, int>>>(), It.IsAny<NameValueCollection>()))
                                     .Returns(Task.CompletedTask);
            var getHandler = new GetByIdsHandler<EntityInt, IEntityInt, int>(_MockServiceCommon.Object,
                                                                             _MockRelatedEntityProvider.Object,
                                                                             _IUrlParameters,
                                                                             _RequestUri);

            // Act
            var actual = await getHandler.HandleAsync(ids);

            // Assert
            Assert.AreEqual(2, actual.Count);
            _MockServiceCommon.Verify(m => m.GetAsync(It.IsAny<List<int>>(), It.IsAny<NameValueCollection>()), Times.Exactly(2));
            _MockRelatedEntityProvider.Verify(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<EntityInt, int>>>(), It.IsAny<NameValueCollection>()), Times.Once);
        }
    }
}