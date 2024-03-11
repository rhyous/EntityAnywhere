using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.WebServices.Common.Tests.EntityInt;
using TId = System.Int32;
using TInterface = Rhyous.EntityAnywhere.WebServices.Common.Tests.IEntityInt;
using Rhyous.EntityAnywhere.Exceptions;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests.Handlers
{
    [TestClass]
    public class DeleteExtensionHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<IGetByIdHandler<TEntity, TInterface, TId>> _MockGetByIdHandler;
        private Mock<INamedFactory<IAdminExtensionEntityClientAsync>> _MockNamedFactory;
        private Mock<IAdminExtensionEntityClientAsync> _MockExtensionEntityClient;
        private Mock<IExtensionEntityList> _MockExtensionEntityList;
        private Mock<IHttpStatusCodeSetter> _MockHttpStatusCodeSetter;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockGetByIdHandler = _MockRepository.Create<IGetByIdHandler<TEntity, TInterface, TId>>();
            _MockNamedFactory = _MockRepository.Create<INamedFactory<IAdminExtensionEntityClientAsync>>();
            _MockExtensionEntityClient = _MockRepository.Create<IAdminExtensionEntityClientAsync>();
            _MockExtensionEntityList = _MockRepository.Create<IExtensionEntityList>();
            _MockHttpStatusCodeSetter = _MockRepository.Create<IHttpStatusCodeSetter>();
        }

        private DeleteExtensionHandler<TEntity, TInterface, int> CreateDeleteExtensionHandler()
        {
            return new DeleteExtensionHandler<TEntity, TInterface, int>(
                _MockGetByIdHandler.Object,
                _MockNamedFactory.Object,
                _MockExtensionEntityList.Object,
                _MockHttpStatusCodeSetter.Object);
        }

        #region HandleAsync
        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task DeleteExtensionHandler_HandleAsync_Id_NullEmptyOrWhitespace_ReturnsNull_WithStatusCodeBadRequest(string id)
        {
            // Arrange
            var deleteExtensionHandler = CreateDeleteExtensionHandler();
            string extensionEntity = "Addendum";

            // Act
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await deleteExtensionHandler.HandleAsync(id, extensionEntity);
            });

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        [StringIsNullEmptyOrWhitespace]
        public async Task DeleteExtensionHandler_HandleAsync_ExtensionEntity_NullEmptyOrWhitespace_ReturnsNull_WithStatusCodeBadRequest(string extensionEntity)
        {
            // Arrange
            var deleteExtensionHandler = CreateDeleteExtensionHandler();
            string id = "100";

            // Act
            await Assert.ThrowsExceptionAsync<RestException>(async () =>
            {
                await deleteExtensionHandler.HandleAsync(id, extensionEntity);
            });

            // Assert
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task DeleteExtensionHandler_HandleAsync_SingleExtension_Test()
        {
            // Arrange
            var deleteExtensionHandler = CreateDeleteExtensionHandler();
            string id = "1";
            string extensionEntity = "AlternateId";

            var entity = new TEntity { Id = id.To<TId>(), Name = "Organization" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            var alternateId = new AlternateId { Id = 100, Entity = entity.Name, EntityId = entity.Id.ToString(), Property = "SapId", Value = "9900001027" };
            var odataAltId = alternateId.AsOdata<AlternateId, long>();
            var relatedAltIds = new RelatedEntityCollection { EntityId = id, Entity = entity.Name, RelatedEntity = nameof(AlternateId) };
            relatedAltIds.Add(odataAltId);
            odataEntity.RelatedEntityCollection.Add(relatedAltIds);

            _MockGetByIdHandler.Setup(m => m.HandleAsync(id)).ReturnsAsync(odataEntity);
            _MockNamedFactory.Setup(m => m.Create(extensionEntity)).Returns(_MockExtensionEntityClient.Object);

            Dictionary<long, bool> ids = new Dictionary<long, bool>();
            ids.Add(100, true);
            _MockExtensionEntityClient.Setup(m => m.DeleteManyAsync(It.IsAny<IEnumerable<long>>(), true)).ReturnsAsync(ids);

            // Act
            var result = await deleteExtensionHandler.HandleAsync(id, extensionEntity);

            // Assert
            Assert.AreEqual(result.Count, 1);
            Assert.IsTrue(result.ContainsKey(100));
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task DeleteExtensionHandler_HandleAsync_AllExtensions_Test()
        {
            // Arrange
            var deleteExtensionHandler = CreateDeleteExtensionHandler();
            string id = "1";
            string extensionEntity = "ALL";

            var entity = new TEntity { Id = id.To<TId>() };
            var odataEntity = entity.AsOdata<TEntity, TId>();

            var addendum = new Addendum { Id = 101, Entity = entity.Name, EntityId = entity.Id.ToString(), Property = "TestAdendum", Value = "Test" };
            var odataAddendum = addendum.AsOdata<Addendum, long>();
            var relatedAddenda = new RelatedEntityCollection { EntityId = id, Entity = entity.Name, RelatedEntity = nameof(Addendum) };
            relatedAddenda.Add(odataAddendum);
            odataEntity.RelatedEntityCollection.Add(relatedAddenda);

            var alternateId = new AlternateId { Id = 100, Entity = entity.Name, EntityId = entity.Id.ToString(), Property = "SapId", Value = "9900001027" };
            var odataAltId = alternateId.AsOdata<AlternateId, long>();
            var relatedAltIds = new RelatedEntityCollection { EntityId = id, Entity = entity.Name, RelatedEntity = nameof(AlternateId) };
            relatedAltIds.Add(odataAltId);
            odataEntity.RelatedEntityCollection.Add(relatedAltIds);

            _MockGetByIdHandler.Setup(m => m.HandleAsync(id)).ReturnsAsync(odataEntity);

            var extensionEntityList = new ExtensionEntityList();
            extensionEntityList.Entities.Add(typeof(Addendum));
            extensionEntityList.Entities.Add(typeof(AlternateId));
            _MockExtensionEntityList.Setup(m => m.EntityNames).Returns(extensionEntityList.EntityNames);

            _MockNamedFactory.Setup(m => m.Create("Addendum")).Returns(_MockExtensionEntityClient.Object);
            _MockNamedFactory.Setup(m => m.Create("AlternateId")).Returns(_MockExtensionEntityClient.Object);

            Dictionary<long, bool> adendeumIds = new Dictionary<long, bool>();
            adendeumIds.Add(101, true);
            _MockExtensionEntityClient.Setup(m => m.DeleteManyAsync(new List<long> { 101 }, true)).ReturnsAsync(adendeumIds);

            Dictionary<long, bool> alternateIdIds = new Dictionary<long, bool>();
            alternateIdIds.Add(100, true);
            _MockExtensionEntityClient.Setup(m => m.DeleteManyAsync(new List<long> { 100 }, true)).ReturnsAsync(alternateIdIds);

            // Act
            var result = await deleteExtensionHandler.HandleAsync(id, extensionEntity);

            // Assert
            Assert.AreEqual(result.Count, 2);
            Assert.IsTrue(result.ContainsKey(100));
            Assert.IsTrue(result.ContainsKey(101));
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
