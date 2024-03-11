using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityInt;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class UpdateExtensionValueHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IGetByIdHandler<TEntity, TInterface, TId>> _MockGetByIdHandler;
        private Mock<INamedFactory<IAdminExtensionEntityClientAsync>> _MockNamedFactory;
        private Mock<IHttpStatusCodeSetter> _MockHttpStatusCodeSetter;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockGetByIdHandler = _MockRepository.Create<IGetByIdHandler<TEntity, TInterface, TId>>();
            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminExtensionEntityClientAsync>>();
            _MockHttpStatusCodeSetter = _MockRepository.Create<IHttpStatusCodeSetter>();
        }

        private UpdateExtensionValueHandler<TEntity, TInterface, TId> CreateUpdateExtensionValueHandler()
        {
            return new UpdateExtensionValueHandler<TEntity, TInterface, TId>(
                _MockGetByIdHandler.Object,
                _MockNamedFactory.Object,
                _MockHttpStatusCodeSetter.Object);
        }

        #region HandleAsync
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UpdateExtensionValueHandler_HandleAsync_id_NullEmptyOrWhitespace_BadRequest(string id)
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "prop1", Value = "val1" };

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UpdateExtensionValueHandler_HandleAsync_extensionEntity_NullEmptyOrWhitespace_BadRequest(string extensionEntity)
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string id = "27";
            PropertyValue propertyValue = null;

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UpdateExtensionValueHandler_HandleAsync_propertyValue_Property_NullEmptyOrWhitespace_BadRequest(string property)
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = property, Value = "val1" };

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UpdateExtensionValueHandler_HandleAsync_propertyValue_Null_BadRequest()
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = null;

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task UpdateExtensionValueHandler_HandleAsync_EntityId_NotFound_SetsNotFound404StatusCode_Test()
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "prop1", Value = "val1" };
            OdataObject<TEntity, TId> odataObject = null;
            _MockGetByIdHandler.Setup(m => m.HandleAsync(id)).ReturnsAsync(odataObject);
            _MockHttpStatusCodeSetter.SetupSet(m => m.StatusCode = HttpStatusCode.NotFound);

            // Act
            var result = await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task UpdateExtensionValueHandler_HandleAsync_ExtensionEntity_NotFound_Sets404(string json)
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "prop1", Value = "val1" };
            var entity = new TEntity { Id = 27, Name = "Some Entity Name" };
            OdataObject<TEntity, TId> odataObject = entity.AsOdata<TEntity, TId>();
            _MockGetByIdHandler.Setup(m => m.HandleAsync(id)).ReturnsAsync(odataObject);
            var mockAddendumClientAsync = _MockRepository.Create<IAdminExtensionEntityClientAsync>();
            _MockNamedFactory.Setup(m => m.Create(extensionEntity))
                             .Returns(mockAddendumClientAsync.Object);
            mockAddendumClientAsync.Setup(m => m.GetByEntityIdentifiersAsync(It.IsAny<IEnumerable<EntityIdentifier>>(), true))
                                   .ReturnsAsync(json);
            _MockHttpStatusCodeSetter.SetupSet(m => m.StatusCode = HttpStatusCode.NotFound);

            // Act
            var result = await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);

            // Assert
            Assert.IsNull(result);
            _MockRepository.VerifyAll();
        }


        [TestMethod]
        public async Task UpdateExtensionValueHandler_HandleAsync_Works()
        {
            // Arrange
            var updateExtensionValueHandler = CreateUpdateExtensionValueHandler();
            string id = "27";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "prop1", Value = "newvalue" };
            var entity = new TEntity { Id = 27, Name = "Some Entity Name" };
            OdataObject<TEntity, TId> odataObject = entity.AsOdata<TEntity, TId>();
            _MockGetByIdHandler.Setup(m => m.HandleAsync(id)).ReturnsAsync(odataObject);
            var mockAddendumClientAsync = _MockRepository.Create<IAdminExtensionEntityClientAsync>();
            _MockNamedFactory.Setup(m => m.Create(extensionEntity))
                             .Returns(mockAddendumClientAsync.Object);
            var addendum = new Addendum
            {
                Id = 1000901,
                Entity = typeof(TEntity).Name,
                EntityId = id,
                Property = propertyValue.Property,
                Value = "oldValue"
            };
            var odataAddendaCollection = new[] { addendum }.AsOdata<Addendum, long>();
            string json = JsonConvert.SerializeObject(odataAddendaCollection);
            mockAddendumClientAsync.Setup(m => m.GetByEntityIdentifiersAsync(It.IsAny<IEnumerable<EntityIdentifier>>(), true))
                                   .ReturnsAsync(json);

            var stringAsJson = JsonConvert.SerializeObject(propertyValue.Value);
            mockAddendumClientAsync.Setup(m => m.UpdatePropertyAsync(addendum.Id.ToString(), 
                                                                     nameof(ExtensionEntity.Value),
                                                                     propertyValue.Value,
                                                                     true))
                                   .ReturnsAsync(stringAsJson);

            // Act
            var result = await updateExtensionValueHandler.HandleAsync(id, extensionEntity, propertyValue);

            // Assert
            Assert.AreEqual(propertyValue.Value, result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
