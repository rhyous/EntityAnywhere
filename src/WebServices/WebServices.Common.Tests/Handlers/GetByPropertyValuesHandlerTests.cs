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
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.MappingEntity1;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IMappingEntity1;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class GetByPropertyValuesHandlerTests
    {
        private MockRepository _MockRepository;
        private Mock<IServiceCommon<TEntity, TInterface, TId>> _MockServiceCommon;
        private Mock<IRelatedEntityProvider<TEntity, TInterface, TId>> _MockRelatedEntityProvider;
        private IUrlParameters _UrlParameters;
        private IRequestUri _RequestUri;
        private Mock<IEntityInfo<TEntity>> _MockEntityInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockServiceCommon = _MockRepository.Create<IServiceCommon<TEntity, TInterface, TId>>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<TEntity, TInterface, TId>>();
            _UrlParameters = new UrlParameters { Collection = new NameValueCollection() };
            _RequestUri = new RequestUri { Uri = new Uri("https://fake.domain.tld/Api/EntityBasicService/EntityBasics(101)") };
            _MockEntityInfo = _MockRepository.Create<IEntityInfo<TEntity>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private GetByPropertyValuesHandler<TEntity, TInterface, TId> CreateGetByPropertyValuesHandler(IUrlParameters urlParameters = null)
        {
            return new GetByPropertyValuesHandler<TEntity, TInterface, TId>(
                _MockServiceCommon.Object,
                _MockRelatedEntityProvider.Object,
                urlParameters ?? _UrlParameters,
                _RequestUri,
                _MockEntityInfo.Object);
        }

        [TestMethod]
        public async Task GetByPropertyValues_Handle_PropertyName_Null_Test()
        {
            // Arrange
            var GetByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string propertyName = null;
            var ids = new List<string>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await GetByPropertyValuesHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByPropertyValues_Handle_PropertyName_Empty_Test()
        {
            // Arrange
            var GetByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string propertyName = "";
            var ids = new List<string>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await GetByPropertyValuesHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByPropertyValues_Handle_PropertyName_Whitespace_Test()
        {
            // Arrange
            var GetByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string propertyName = "    ";
            var ids = new List<string>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await GetByPropertyValuesHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByPropertyValues_Handle_ids_Null_Test()
        {
            // Arrange
            var GetByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string propertyName = "EntityIntId";
            List<string> ids = null;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () => 
            {
                await GetByPropertyValuesHandler.HandleAsync(propertyName, ids); 
            });
        }

        [TestMethod]
        public async Task GetByPropertyValues_Handle_ids_Empty_Test()
        {
            // Arrange
            var GetByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string propertyName = "EntityIntId";
            var ids = new List<string>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await GetByPropertyValuesHandler.HandleAsync(propertyName, ids);
            });
        }

        [TestMethod]
        public async Task GetByPropertyValues_Handle_ids_Valid_Test()
        {
            // Arrange
            var nvc = new NameValueCollection { { "$top", "1" } };
            var GetByPropertyValuesHandler = CreateGetByPropertyValuesHandler();
            string propertyName = "EntityIntId";
            var props = typeof(TEntity).GetProperties()
                                       .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            _MockEntityInfo.Setup(m => m.Properties).Returns(props);
            var ids = new List<string> { "27", "128" };
            var mappedEntities = new List<TEntity>
            {
                new TEntity { Id = 10, EntityIntId =  27, EntityBasicId = 507 },
                new TEntity { Id = 20, EntityIntId = 128, EntityBasicId = 509}
            };
            _MockServiceCommon.SetupSequence(m => m.GetAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<NameValueCollection>()))
                              .ReturnsAsync(mappedEntities.AsQueryable())
                              .ReturnsAsync(mappedEntities.Take(1).AsQueryable());
            _MockRelatedEntityProvider.Setup(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()))
                                     .Returns(Task.CompletedTask);

            // Act
            var actual = await GetByPropertyValuesHandler.HandleAsync(propertyName, ids);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual.TotalCount);
            _MockServiceCommon.Verify(m => m.GetAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<NameValueCollection>()), Times.Exactly(2));
            _MockRelatedEntityProvider.Verify(m => m.ProvideAsync(It.IsAny<IEnumerable<OdataObject<TEntity, TId>>>(), It.IsAny<NameValueCollection>()), Times.Once);
        }
    }
}
