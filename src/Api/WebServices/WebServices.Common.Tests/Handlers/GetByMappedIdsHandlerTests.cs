using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.RelatedEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.MappingEntity1;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IMappingEntity1;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetByMappedIdsHandlerTests
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
            _IUrlParameters = new UrlParameters();
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics(101)") };

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetByMappedIdsHandler<TEntity, TInterface, TId, int> CreateGetByMappedIdsHandler(IUrlParameters urlParameters = null)
        {
            return new GetByMappedIdsHandler<TEntity, TInterface, TId, int>(
                _MockServiceCommon.Object,
                _MockRelatedEntityProvider.Object,
                urlParameters ?? _IUrlParameters,
                _RequestUri);
        }

        [TestMethod]
        public async Task GetByMappedIds_Handle_PropertyName_Null_Test()
        {
            // Arrange
            var getByMappedIdsHandler = CreateGetByMappedIdsHandler();
            string propertyName = null;
            var ids = new List<int>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await getByMappedIdsHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByMappedIds_Handle_PropertyName_Empty_Test()
        {
            var getByMappedIdsHandler = CreateGetByMappedIdsHandler();
            string propertyName = "";
            var ids = new List<int>();
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await getByMappedIdsHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByMappedIds_Handle_PropertyName_Whitespace_Test()
        {
            // Arrange
            var getByMappedIdsHandler = CreateGetByMappedIdsHandler();
            string propertyName = "    ";
            var ids = new List<int>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () => 
            {
                await getByMappedIdsHandler.HandleAsync(propertyName, ids); 
            });
        }

        [TestMethod]
        public async Task GetByMappedIds_Handle_ids_Null_Test()
        {
            // Arrange
            var getByMappedIdsHandler = CreateGetByMappedIdsHandler();
            string propertyName = "EntityIntId";
            List<int> ids = null;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await getByMappedIdsHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByMappedIds_Handle_ids_Empty_Test()
        {
            // Arrange
            var getByMappedIdsHandler = CreateGetByMappedIdsHandler();
            string propertyName = "EntityIntId";
            var ids = new List<int>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await getByMappedIdsHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByMappedIds_Handle_ids_Valid_Test()
        {
            // Arrange
            var nvc = new NameValueCollection { { "$top", "1" } };
            var getByMappedIdsHandler = CreateGetByMappedIdsHandler();
            string propertyName = "EntityIntId";
            var ids = new List<int> { 27, 128 };
            var mappedEntities = new List<TEntity>
            {
                new TEntity { Id = 10, EntityIntId =  27, EntityBasicId = 507 },
                new TEntity { Id = 20, EntityIntId = 128, EntityBasicId = 509}
            };
            _MockServiceCommon.SetupSequence(m => m.Get(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<int>(), It.IsAny<int>()))
                              .Returns(mappedEntities.AsQueryable())
                              .Returns(mappedEntities.Take(1).AsQueryable());
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                      .Returns(Task.CompletedTask);

            // Act
            var actual = await getByMappedIdsHandler.HandleAsync(propertyName, ids);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual.TotalCount);
            _MockServiceCommon.Verify(m => m.Get(It.IsAny<Expression<Func<TEntity, bool>>>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            _MockRelatedEntityProvider.Verify(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()), Times.Once);
        }
    }
}
