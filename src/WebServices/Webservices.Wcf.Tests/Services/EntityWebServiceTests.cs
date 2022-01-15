using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.WebServices;
using System;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityString;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityString;
using TId = System.String;
using System.Collections.Generic;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Services
{
    [TestClass]
    public class EntityWebServiceTests
    {
        private MockRepository _MockRepository;

        private Mock<IRestHandlerProvider<TEntity, TInterface, TId>> _MockRestHandlerProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRestHandlerProvider = _MockRepository.Create<IRestHandlerProvider<TEntity, TInterface, TId>>();
        }

        private EntityWebService<TEntity, TInterface, TId> CreateService()
        {
            return new EntityWebService<TEntity, TInterface, TId>(
                _MockRestHandlerProvider.Object);
        }

        #region UpdatePropertyAsync
        [TestMethod]
        public async Task EntityWebService_UpdateProperty_RestHandlerProvider_UpdatePropertyHandler_IsCalled()
        {
            // Arrange
            var entityWebService = CreateService();
            string id = "1";
            string property = "Name";
            string value = "Product Name TEST";
            var mockUpdatePropertyHandler = _MockRepository.Create<IUpdatePropertyHandler<TEntity, TInterface, TId>>();
            mockUpdatePropertyHandler.Setup(m => m.Handle(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                     .ReturnsAsync("FakePropertyValue");

            _MockRestHandlerProvider.Setup(m => m.UpdatePropertyHandler).Returns(mockUpdatePropertyHandler.Object);

            // Act
            await entityWebService.UpdatePropertyAsync(id, property, value);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Post
        [TestMethod]
        public async Task EntityWebService_Post_PostHandler_Handle_Called()
        {
            // Arrange
            var entityWebService = CreateService();
            var entities = new List<TEntity>
            {
                new TEntity { Id = "1", Name = "N1" },
                new TEntity { Id = "2", Name = "N2" },
            };
            var odataEntities = entities.AsOdata<TEntity, TId>();
            var mockPostHandler = _MockRepository.Create<IPostHandler<TEntity, TInterface, TId>>();
            mockPostHandler.Setup(m => m.HandleAsync(It.IsAny<List<TEntity>>()))
                                     .ReturnsAsync(odataEntities);
            _MockRestHandlerProvider.Setup(m => m.PostHandler).Returns(mockPostHandler.Object);

            // Act
            var actual = await entityWebService.PostAsync(entities);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region PostOneWay
        [TestMethod]
        public async Task EntityWebService_PostOneWay_PostHandler_Handle_Called()
        {
            // Arrange
            var entityWebService = CreateService();
            var entities = new List<TEntity>
            {
                new TEntity { Id = "1", Name = "N1" },
                new TEntity { Id = "2", Name = "N2" },
            };
            var odataEntities = entities.AsOdata<TEntity, TId>();
            var mockPostHandler = _MockRepository.Create<IPostHandler<TEntity, TInterface, TId>>();
            mockPostHandler.Setup(m => m.HandleAsync(It.IsAny<List<TEntity>>()))
                                     .ReturnsAsync(odataEntities);
            _MockRestHandlerProvider.Setup(m => m.PostHandler).Returns(mockPostHandler.Object);

            // Act
            await entityWebService.PostOneWayAsync(entities);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Patch
        [TestMethod]
        public void EntityWebService_Patch_PatchHandler_Handle_Called()
        {
            // Arrange
            var entityWebService = CreateService();
            var entity = new TEntity { Id = "1", Name = "N1a" };
            var priorEntity = new TEntity { Id = "1", Name = "N1" };
            var patchedEntity = new PatchedEntity<TEntity, TId>
            {
                Entity = entity,
                ChangedProperties = new HashSet<string> { "Name" }
            };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            var mockPatchHandler = _MockRepository.Create<IPatchHandler<TEntity, TInterface, TId>>();
            mockPatchHandler.Setup(m => m.HandleAsync(It.IsAny<string>(), It.IsAny<PatchedEntity<TEntity, TId>>()))
                                     .ReturnsAsync(odataEntity);

            _MockRestHandlerProvider.Setup(m => m.PatchHandler).Returns(mockPatchHandler.Object);

            // Act
            var actual = entityWebService.PatchAsync(entity.Id, patchedEntity);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region PatchMany
        [TestMethod]
        public async Task EntityWebService_PatchMany_PatchHandler_Handle_Called()
        {
            // Arrange
            var service = CreateService();
            var entity = new TEntity { Id = "1", Name = "N1a" };
            var odataObjectCollection = new[] { entity }.AsOdata<TEntity, TId>();
            var patchedEntityCollection = new PatchedEntityCollection<TEntity, TId>();
            var mockPatchHandler = _MockRepository.Create<IPatchHandler<TEntity, TInterface, TId>>();
            mockPatchHandler.Setup(m => m.Handle(patchedEntityCollection))
                            .ReturnsAsync(odataObjectCollection);
            _MockRestHandlerProvider.Setup(m => m.PatchHandler).Returns(mockPatchHandler.Object);

            // Act
            var result = await service.PatchManyAsync(patchedEntityCollection);

            // Assert
            Assert.AreEqual(odataObjectCollection, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Put
        [TestMethod]
        public void EntityWebService_Put_PutHandler_Handle_Called()
        {
            // Arrange
            var entityWebService = CreateService();
            var entity = new TEntity { Id = "1", Name = "N1a" };
            var priorEntity = new TEntity { Id = "1", Name = "N1" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            var mockPutHandler = _MockRepository.Create<IPutHandler<TEntity, TInterface, TId>>();
            mockPutHandler.Setup(m => m.Handle(It.IsAny<string>(), It.IsAny<TEntity>()))
                                     .ReturnsAsync(odataEntity);

            _MockRestHandlerProvider.Setup(m => m.PutHandler).Returns(mockPutHandler.Object);

            // Act
            var actual = entityWebService.PutAsync(entity.Id, entity);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region Delete

        [TestMethod]
        public void EntityWebService_Delete_DeleteHandler_Handle_Called()
        {
            // Arrange
            var entityWebService = CreateService();
            var mockDeleteHandler = new Mock<IDeleteHandler<TEntity, TInterface, TId>>();
            mockDeleteHandler.Setup(m => m.Handle(It.IsAny<string>()))
                                     .Returns(true);

            _MockRestHandlerProvider.Setup(m => m.DeleteHandler).Returns(mockDeleteHandler.Object);


            // Act
            var actual = entityWebService.Delete("1013");

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region DeleteManyAsync
        [TestMethod]
        public async Task EntityWebService_DeleteMany_DeleteManyHandler_Handle_Called()
        {
            // Arrange
            var service = CreateService();
            var ids = new List<string> { "190", "201" };
            var mockDeleteManyHandler = new Mock<IDeleteManyHandler<TEntity, TInterface, TId>>();
            var returnDict = new Dictionary<TId, bool> { { ids[0], true }, { ids[1], true } };
            mockDeleteManyHandler.Setup(m => m.HandleAsync(ids))
                                     .ReturnsAsync(returnDict);

            _MockRestHandlerProvider.Setup(m => m.DeleteManyHandler).Returns(mockDeleteManyHandler.Object);

            // Act
            var result = await service.DeleteManyAsync(ids);

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region GenerateRepository
        [TestMethod]
        public void EntityWebService_GenerateRepository_ServiceCommon_GeneratorRepository_Called()
        {
            // Arrange
            var service = CreateService();
            var repositoryGenerationResult = new RepositoryGenerationResult();
            var mockGenerateRepositoryHandler = _MockRepository.Create<IGenerateRepositoryHandler<TEntity, TInterface, TId>>();
            _MockRestHandlerProvider.Setup(m => m.GenerateRepositoryHandler).Returns(mockGenerateRepositoryHandler.Object);
            mockGenerateRepositoryHandler.Setup(m => m.GenerateRepository()).Returns(repositoryGenerationResult);

            // Act
            var result = service.GenerateRepository();

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region InsertSeedData
        [TestMethod]
        public void EntityWebService_InsertSeedData_ServiceCommon_InsertSeedData_Called()
        {
            // Arrange
            var service = CreateService();
            var repositorySeedResult = new RepositorySeedResult();
            var mockInsertSeedDataHandler = _MockRepository.Create<IInsertSeedDataHandler<TEntity, TInterface, TId>>();
            _MockRestHandlerProvider.Setup(m => m.InsertSeedDataHandler).Returns(mockInsertSeedDataHandler.Object);
            mockInsertSeedDataHandler.Setup(m => m.InsertSeedData()).Returns(repositorySeedResult);

            // Act
            var result = service.InsertSeedData();

            // Assert
            _MockRepository.VerifyAll();
        }
        #endregion

        #region PostExtensionAsync
        [TestMethod]
        public async Task EntityWebService_PostExtensionAsync_IPostExtensionHandler_HandleAsync_Called()
        {
            // Arrange
            var service = CreateService();
            TId id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "prop1", Value = "val1" };

            var mockPostExtensionHandler = new Mock<IPostExtensionHandler<TEntity, TInterface, TId>>();
            var entity = new TEntity { Id = id, Name = "Some Entity" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            var addendum = new Addendum
            {
                Id = 100609,
                Entity = typeof(TEntity).Name,
                EntityId = id,
                Property = propertyValue.Property,
                Value = propertyValue.Value
            };
            var odataAddendum = new[] { addendum }.AsOdata<Addendum, long>();
            odataEntity.RelatedEntityCollection.Add(odataAddendum);
            mockPostExtensionHandler.Setup(m => m.HandleAsync(id, extensionEntity, propertyValue))
                                    .ReturnsAsync(odataEntity);

            _MockRestHandlerProvider.Setup(m => m.PostExtensionHandler).Returns(mockPostExtensionHandler.Object);

            // Act
            var result = await service.PostExtensionAsync(id, extensionEntity, propertyValue);

            // Assert
            Assert.AreEqual(odataEntity, result);
            _MockRepository.VerifyAll();
        }
        #endregion

        #region UpdateExtensionValueAsync
        [TestMethod]
        public async Task EntityWebService_UpdateExtensionValueAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = CreateService();
            TId id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "prop1", Value = "newval1" };
            var mockUpdateExtensionValueHandler = new Mock<IUpdateExtensionValueHandler<TEntity, TInterface, TId>>();
            mockUpdateExtensionValueHandler.Setup(m => m.HandleAsync(id, extensionEntity, propertyValue))
                                           .ReturnsAsync(propertyValue.Value);

            _MockRestHandlerProvider.Setup(m => m.UpdateExtensionValueHandler).Returns(mockUpdateExtensionValueHandler.Object);

            // Act
            var result = await service.UpdateExtensionValueAsync(id, extensionEntity, propertyValue);

            // Assert
            Assert.AreEqual(propertyValue.Value, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
