using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TEntity = Rhyous.EntityAnywhere.Clients2.Common.Tests.EntityInt;
using TId = System.Int32;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    [TestClass]
    public class EntityClientAsyncTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientConnectionSettings<TEntity>> _MockEntityClientSettings;
        private Mock<IHttpClientRunner> _MockHttpClientRunner;
        private MockSetupHelper _MockSetupHelper;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);
            _MockEntityClientSettings = _MockRepository.Create<IEntityClientConnectionSettings<TEntity>>();
            _MockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
            _MockSetupHelper = new MockSetupHelper(_MockEntityClientSettings,
                                                   _MockHttpClientRunner);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private IEntityClientAsync<TEntity, TId> CreateEntityClientAsync()
        {
            return new EntityClientAsync<TEntity, TId>(
                _MockEntityClientSettings.Object,
                _MockHttpClientRunner.Object);
        }

        #region Constructor tests
        [TestMethod]
        public void EntityClientAsync_Constructor_HttpClientRunner_Null_Test()
        {

            Assert.ThrowsException<ArgumentNullException>(() =>
             {
                 new EntityClientAsync<TEntity, TId>(_MockEntityClientSettings.Object,
                                                     null);
             });
        }

        #endregion

        #region DeleteAsync
        [TestMethod]
        public async Task EntityClientAsync_DeleteAsync_id_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.DeleteAsync(
                    id,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteAsync_True()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "11";
            bool forwardExceptions = false;

            _MockSetupHelper.SetupDeleteMocks(true);

            // Act
            var result = await entityClientAsync.DeleteAsync(
                id,
                forwardExceptions);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteAsync_False()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "11";
            bool forwardExceptions = false;

            _MockSetupHelper.SetupDeleteMocks(false);

            // Act
            var result = await entityClientAsync.DeleteAsync(
                id,
                forwardExceptions);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region GetAllAsync
        [TestMethod]
        public async Task EntityClientAsync_GetAllAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAllAsync(forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region GetAsync
        [TestMethod]
        public async Task EntityClientAsync_GetAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            TId id = 11;
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAsync(
                id,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "11";
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = null;
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_UrlParam_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "11";
            string urlParameters = "a=b";
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_ByAltKey_UrlParam_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "$Alt.$Key.11";
            string urlParameters = "a=b";
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_ByAltId_UrlParam_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "$Alt.SF18Id.oooOOO111222333444";
            string urlParameters = "a=b";
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_ByAltId_NotDisambiguated_UrlParam_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "oooOOO111222333444";
            string urlParameters = "a=b";
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }
        #endregion

        #region GetByAlternateKeyAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateKeyAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = "27";
            bool forwardExceptions = false;
            var odataObject = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByAlternateKeyAsync(
                altKey,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataObject, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateKeyAsync_Null_AltKey_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await entityClientAsync.GetByAlternateKeyAsync(
                altKey,
                forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateKeyAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = "27";
            string urlParameters = "?$expand=Entity2";
            bool forwardExceptions = false;
            var odataObject = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByAlternateKeyAsync(
                altKey,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataObject, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateIdAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = "27";
            string urlParameters = null;
            bool forwardExceptions = false;
            var odataObject = _MockSetupHelper.SetupRunAndDeserializeSingleMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByAlternateIdAsync(
                altKey,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataObject, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateIdAsync_Id_IsNull_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = null;
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await entityClientAsync.GetByAlternateIdAsync(
                altKey,
                urlParameters,
                forwardExceptions);
            });
        }
        #endregion

        [TestMethod]
        public async Task EntityClientAsync_GetByCustomUrlAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = "some/url/part";
            bool forwardExceptions = false;
            var odataObjects = _MockSetupHelper.SetupRunAndDeserializeCollectionMock();
            _MockSetupHelper.SetupServiceUrl();

            // Act
            var result = await entityClientAsync.GetByCustomUrlAsync(
                urlPart,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataObjects, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByCustomUrlAsync_urlPart_null_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetByCustomUrlAsync(
                    urlPart,
                    forwardExceptions);
            });
        }

        #region CallByCustomUrlAsync
        [TestMethod]
        public async Task EntityClientAsync_CallByCustomUrlAsync_UrlPart_Null_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = null;
            HttpMethod httpMethod = null;
            object content = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.CallByCustomUrlAsync(
                                                    urlPart,
                                                    httpMethod,
                                                    content,
                                                    forwardExceptions);
            });

        }

        [TestMethod]
        public async Task EntityClientAsync_CallByCustomUrlAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = "some/service";
            HttpMethod httpMethod = null;
            object content = null;
            bool forwardExceptions = false;
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionWithObjectContent();
            _MockSetupHelper.SetupServiceUrl();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.CallByCustomUrlAsync(
                                                    urlPart,
                                                    httpMethod,
                                                    content,
                                                    forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region GetObjectByCustomUrlAsync
        [TestMethod]
        public async Task EntityClientAsync_GetObjectByCustomUrlAsync_UrlPart_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = null;
            HttpMethod httpMethod = null;
            object content = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetObjectByCustomUrlAsync(
                urlPart,
                httpMethod,
                content,
                forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetObjectByCustomUrlAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = "some/service";
            HttpMethod httpMethod = null;
            object content = null;
            bool forwardExceptions = false;
            var odataEntity = _MockSetupHelper.SetupRunAndDeserializeSingleWithObjectContent();
            _MockSetupHelper.SetupServiceUrl();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.GetObjectByCustomUrlAsync(
               urlPart,
               httpMethod,
               content,
               forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }
        #endregion

        [TestMethod]
        public async Task EntityClientAsync_GetByQueryParametersAsync_Null_QueryParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string queryParameters = null;
            bool forwardExceptions = false;
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync((OdataObjectCollection<TEntity, TId>)null);
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByQueryParametersAsync(
                queryParameters,
                forwardExceptions);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByQueryParametersAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string queryParameters = null;
            bool forwardExceptions = false;
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByQueryParametersAsync(
                queryParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }

        #region GetByIdsAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByIdsAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<TId> ids = new[] { 27, 127, 1027 };
            bool forwardExceptions = false;
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntities = new[] { entity }.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<IEnumerable<TId>, OdataObjectCollection<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<IEnumerable<TId>>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);

            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.GetByIdsAsync(
                ids,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByIdsAsync_Ids_NullTest()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<TId> ids = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetByIdsAsync(
                   ids,
                   forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByIdsAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<TId> ids = new[] { 27, 127, 1027 };
            string urlParameters = "$expand=entity2";
            bool forwardExceptions = false;
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntities = new[] { entity }.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<object, OdataObjectCollection<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<object>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);

            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.GetByIdsAsync(
                ids,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region GetByPropertyValues
        [TestMethod]
        public async Task EntityClientAsync_GetByPropertyValuesAsync_Property_Null_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = null;
            IEnumerable<string> values = new[] { "1", "2", "3" };
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetByPropertyValuesAsync(
                    property,
                    values,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByPropertyValuesAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = "Name";
            IEnumerable<string> values = new[] { "1", "2", "3" };
            bool forwardExceptions = false;
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionWithObjectContent();
            _MockSetupHelper.SetupJsonSerializerSettings();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByPropertyValuesAsync(
                property,
                values,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByPropertyValuesAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = "Name";
            IEnumerable<string> values = new[] { "1", "2", "3" };
            string urlParameters = "$expand=Entity2";
            bool forwardExceptions = false;
            var odataEntities = _MockSetupHelper.SetupRunAndDeserializeCollectionWithObjectContent();
            _MockSetupHelper.SetupJsonSerializerSettings();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetByPropertyValuesAsync(
                property,
                values,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region GetPropertyAsync
        [TestMethod]
        public async Task EntityClientAsync_GetPropertyAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            string property = "Name";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetPropertyAsync(
                   id,
                   property,
                   forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetPropertyAsync_Property_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "123";
            string property = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetPropertyAsync(
                   id,
                   property,
                   forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetPropertyAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "123";
            string property = "Name";
            bool forwardExceptions = false;
            var propertyValue = "value";
            _MockHttpClientRunner.Setup(m => m.Run(
                                   It.IsAny<HttpMethod>(),
                                   It.IsAny<string>(),
                                   It.IsAny<bool>()))
                     .ReturnsAsync(propertyValue);
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetPropertyAsync(
                id,
                property,
                forwardExceptions);

            // Assert
            Assert.AreEqual(propertyValue, result);
        }
        #endregion

        [TestMethod]
        public async Task EntityClientAsync_GetMetadataAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            CsdlEntity csdl = _MockSetupHelper.SetupRunAndDeserializeGeneric<CsdlEntity>();
            _MockSetupHelper.SetupServiceUrl();

            // Act
            var result = await entityClientAsync.GetMetadataAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(csdl, result);
        }

        #region PatchAsync
        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            PatchedEntity<TEntity, TId> patchedEntity = new PatchedEntity<TEntity, TId>();
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PatchAsync(
                id,
                patchedEntity,
                forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_PatchedEntity_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            PatchedEntity<TEntity, TId> patchedEntity = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PatchAsync(
                id,
                patchedEntity,
                forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "1027";
            PatchedEntity<TEntity, TId> patchedEntity = new PatchedEntity<TEntity, TId>();
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<PatchedEntity<TEntity, TId>, OdataObject<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<PatchedEntity<TEntity, TId>>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntity);

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                patchedEntity,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "1027";
            PatchedEntity<TEntity, TId> patchedEntity = new PatchedEntity<TEntity, TId>();
            string urlParameters = "$expand=entity2";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<PatchedEntity<TEntity, TId>, OdataObject<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<PatchedEntity<TEntity, TId>>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntity);

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                patchedEntity,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }
        #endregion

        #region PostAsync
        [TestMethod]
        public async Task EntityClientAsync_PostAsync_Null_Entities()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<TEntity> entities = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PostAsync(
                entities,
                forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PostAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            var entity = new TEntity { Id = 27, Name = "E27" };
            IEnumerable<TEntity> entities = new[] { entity };
            bool forwardExceptions = false;
            var odataEntities = entities.AsOdata<TEntity, int>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<IEnumerable<TEntity>, OdataObjectCollection<TEntity, TId>>(
                                   It.IsAny<HttpMethod>(),
                                   It.IsAny<string>(),
                                   It.IsAny<IEnumerable<TEntity>>(),
                                   It.IsAny<JsonSerializerSettings>(),
                                   It.IsAny<bool>()))
                     .ReturnsAsync(odataEntities);
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.PostAsync(
                entities,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PostAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            var entity = new TEntity { Id = 27, Name = "E27" };
            IEnumerable<TEntity> entities = new[] { entity };
            string urlParameters = "$Expand=Entity2";
            bool forwardExceptions = false;
            var odataEntities = entities.AsOdata<TEntity, int>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<IEnumerable<TEntity>, OdataObjectCollection<TEntity, TId>>(
                                   It.IsAny<HttpMethod>(),
                                   It.IsAny<string>(),
                                   It.IsAny<IEnumerable<TEntity>>(),
                                   It.IsAny<JsonSerializerSettings>(),
                                   It.IsAny<bool>()))
                     .ReturnsAsync(odataEntities);
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.PostAsync(
                entities,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
        #endregion

        #region PutAsync
        [TestMethod]
        public async Task EntityClientAsync_PutAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            TEntity entity = new TEntity { Id = 27, Name = "E27" };
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PutAsync(
                    id,
                    entity,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_Null_Entity()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            TEntity entity = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PutAsync(
                    id,
                    entity,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            TEntity entity = new TEntity { Id = 27, Name = "E27" };
            var odataEntity = entity.AsOdata<TEntity, int>();
            bool forwardExceptions = false;
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<TEntity, OdataObject<TEntity, TId>>(
                                       It.IsAny<HttpMethod>(),
                                       It.IsAny<string>(),
                                       It.IsAny<TEntity>(),
                                       It.IsAny<JsonSerializerSettings>(),
                                       It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntity);
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                entity,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            TEntity entity = new TEntity { Id = 27, Name = "E27" };
            var odataEntity = entity.AsOdata<TEntity, int>();
            string urlParameters = "$expand=Entity2";
            bool forwardExceptions = false;
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<TEntity, OdataObject<TEntity, TId>>(
                                        It.IsAny<HttpMethod>(),
                                        It.IsAny<string>(),
                                        It.IsAny<TEntity>(),
                                        It.IsAny<JsonSerializerSettings>(),
                                        It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntity);
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                entity,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(odataEntity, result);
        }
        #endregion

        #region UpdatePropertyAsync
        [TestMethod]
        public async Task EntityClientAsync_UpdatePropertyAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            string property = "Name";
            string value = "somenewname";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.UpdatePropertyAsync(
                    id,
                    property,
                    value,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_UpdatePropertyAsync_Property_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            string property = null;
            string value = "somenewname";
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.UpdatePropertyAsync(
                    id,
                    property,
                    value,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_UpdatePropertyAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            var id = "27";
            var property = "Name";
            var value = "somenewname";
            var url = "https://eaf.domain.tld/ThisService/EntityInts(27)/Name";
            var forwardExceptions = false;
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<string, string>(
                                   HttpMethod.Put,
                                   url,
                                   value,
                                   It.IsAny<JsonSerializerSettings>(),
                                   forwardExceptions))
                     .ReturnsAsync(value);
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.UpdatePropertyAsync(
                id,
                property,
                value,
                forwardExceptions);

            // Assert
            Assert.AreEqual(result, value);
        }
        #endregion

        #region GetCountAsync
        [TestMethod]
        public async Task EntityClientAsync_GetCountAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<int>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(10);

            // Act
            var result = await entityClientAsync.GetCountAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetCountAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupRunAndDeserializeGeneric<int>(10);

            // Act
            var result = await entityClientAsync.GetCountAsync(
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetCountAsync_Url_ParamsNotEmpty_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlParameters = "$filter=Contains(Name, abc)";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            int expectedCount = 10;
            string actualUrl = "";
            string expectedUrl = "https://eaf.domain.tld/ThisService/EntityInts?$filter=Contains(Name, abc)&$count";
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<int>(
                                   It.IsAny<HttpMethod>(),
                                   It.IsAny<string>(),
                                   It.IsAny<bool>()))
                     .ReturnsAsync((HttpMethod m, string url, bool b) =>
                     {
                         actualUrl = url;
                         return expectedCount;
                     });

            // Act
            var result = await entityClientAsync.GetCountAsync(
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(expectedCount, result);
            Assert.AreEqual(expectedUrl, actualUrl);
        }
        #endregion

        [TestMethod]
        public async Task EntityClientAsync_GenerateRepositoryAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            var repositoryGenerationResult = new RepositoryGenerationResult();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<RepositoryGenerationResult>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(repositoryGenerationResult);
            _MockSetupHelper.SetupServiceUrl();

            // Act
            var result = await entityClientAsync.GenerateRepositoryAsync(forwardExceptions);

            // Assert
            Assert.AreEqual(repositoryGenerationResult, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_InsertSeedDataAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            var seedResult = new RepositorySeedResult();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<RepositorySeedResult>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                  .ReturnsAsync(seedResult);
            _MockSetupHelper.SetupServiceUrl();

            // Act
            var result = await entityClientAsync.InsertSeedDataAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(seedResult, result);
        }
    }
}
