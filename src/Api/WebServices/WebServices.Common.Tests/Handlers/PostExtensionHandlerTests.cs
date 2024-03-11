using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.Wrappers;
using System.Net;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityInt;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Exceptions;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class PostExtensionHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IGetByIdHandler<TEntity, TInterface, TId>> _MockGetByIdHandler;
        private Mock<INamedFactory<IAdminExtensionEntityClientAsync>> _MockNamedFactory;
        private Mock<IAdminExtensionEntityClientAsync> _MockExtensionEntityClient;
        private Mock<IHttpStatusCodeSetter> _MockHttpStatusCodeSetter;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockGetByIdHandler = _MockRepository.Create<IGetByIdHandler<TEntity, TInterface, TId>>();
            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminExtensionEntityClientAsync>>();
            _MockExtensionEntityClient = _MockRepository.Create<IAdminExtensionEntityClientAsync>();
            _MockHttpStatusCodeSetter = _MockRepository.Create<IHttpStatusCodeSetter>();
        }

        private PostExtensionHandler<TEntity, TInterface, int> CreatePostExtensionHandler()
        {
            return new PostExtensionHandler<TEntity, TInterface, int>(
                _MockGetByIdHandler.Object,
                _MockNamedFactory.Object,
                _MockHttpStatusCodeSetter.Object);
        }

        #region HandleAsync
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task PostExtensionHandler_HandleAsync_Id_NullEmptyOrWhitespace_ReturnsNull_WithStatusCodeBadRequest(string id)
        {
            // Arrange
            var postExtensionHandler = CreatePostExtensionHandler();
            string extensionEntity = "Addendum";
            var propertyValue = new PropertyValue { Property = "Prop1", Value = "Val1" };

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await postExtensionHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task PostExtensionHandler_HandleAsync_ExtensionEntity_NullEmptyOrWhitespace_ReturnsNull_WithStatusCodeBadRequest(string extensionEntity)
        {
            // Arrange
            var postExtensionHandler = CreatePostExtensionHandler();
            string id = "100";
            PropertyValue propertyValue = new PropertyValue { Property = "Prop1", Value = "Val1" };

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await postExtensionHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task PostExtensionHandler_HandleAsync_PropertyValue_NullEmptyOrWhitespace_ReturnsNull_WithStatusCodeBadRequest()
        {
            // Arrange
            var postExtensionHandler = CreatePostExtensionHandler();
            string id = "100";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = null;

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await postExtensionHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task PostExtensionHandler_HandleAsync_PropertyValue_Property_NullEmptyOrWhitespace_ReturnsNull_WithStatusCodeBadRequest(string property)
        {
            // Arrange
            var postExtensionHandler = CreatePostExtensionHandler();
            string id = "100";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = property, Value = "Val1" };

            // Act
            var ex = await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await postExtensionHandler.HandleAsync(id, extensionEntity, propertyValue);
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, ex.StatusCode);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task PostExtensionHandler_HandleAsync_Test()
        {
            // Arrange
            var postExtensionHandler = CreatePostExtensionHandler();
            string id = "100";
            string extensionEntity = "Addendum";
            PropertyValue propertyValue = new PropertyValue { Property = "Prop1", Value = "Val1" };
            var entity = new TEntity { Id = id.To<TId>() };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            _MockGetByIdHandler.Setup(m => m.HandleAsync(id)).ReturnsAsync(odataEntity);
            _MockNamedFactory.Setup(m => m.Create(extensionEntity)).Returns(_MockExtensionEntityClient.Object);
            var addendum = new Addendum { Id = 100, Entity = typeof(TEntity).Name, Property = propertyValue.Property, Value = propertyValue.Value };
            var odataCollectionAddendum = new[] { addendum }.AsOdata<Addendum, long>();
            var json = JsonConvert.SerializeObject(odataCollectionAddendum);
            _MockExtensionEntityClient.Setup(m => m.PostAsync(It.IsAny<object>(), true)).ReturnsAsync(json);

            // Act
            var result = await postExtensionHandler.HandleAsync(id, extensionEntity, propertyValue);

            // Assert
            Assert.IsNotNull(result);
            var relatedEntityCollectionAddendum = result.GetRelatedEntityCollection<Addendum, long>();
            Assert.IsNotNull(relatedEntityCollectionAddendum);
            Assert.AreEqual(1, relatedEntityCollectionAddendum.Count);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
