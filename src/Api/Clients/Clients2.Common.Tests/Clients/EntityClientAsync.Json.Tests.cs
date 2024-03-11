using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    [TestClass]
    public class EntityClientAsyncJsonTests
    {
        private MockRepository mockRepository;

        private Mock<IEntityClientConnectionSettings> _MockEntityClientConnectionSettings;
        private Mock<IHttpClientRunner> _MockHttpClientRunner; 
        private MockSetupHelper _MockSetupHelper;


        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConnectionSettings = mockRepository.Create<IEntityClientConnectionSettings>();
            _MockHttpClientRunner = mockRepository.Create<IHttpClientRunner>();
            _MockSetupHelper = new MockSetupHelper(_MockEntityClientConnectionSettings,
                                                   _MockHttpClientRunner);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            mockRepository.VerifyAll();
        }

        private IEntityClientAsync CreateEntityClientAsync()
        {
            return new EntityClientAsync(
                _MockEntityClientConnectionSettings.Object,
                _MockHttpClientRunner.Object);
        }

        #region Constructor tests
        [TestMethod]
        public void EntityClientAsync_Constructor_HttpClientRunner_Null_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new EntityClientAsync(_MockEntityClientConnectionSettings.Object,
                                      null);
            });
        }
        #endregion

        #region DeleteAsync
        [TestMethod]
        public async Task EntityClientAsync_DeleteAsync_Id_Null()
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
        public async Task EntityClientAsync_DeleteAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupDeleteMocks(true);

            // Act
            var result = await entityClientAsync.DeleteAsync(
                id,
                forwardExceptions);

            // Assert
            Assert.IsTrue(result);
        }
        #endregion

        [TestMethod]
        public async Task EntityClientAsync_GetAllAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            string json = _MockSetupHelper.SetupRunMock();
            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.GetAllAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        
        #region GetAsync
        [TestMethod]
        public async Task EntityClientAsync_GetAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetAsync(
                    idOrName,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "27";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            string json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "27";
            string urlParameters = "$expand=none";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            string json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_ByAltKey_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "$Alt.$Key.Id27";
            string urlParameters = "$expand=none";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            string json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetAsync_ByAltId_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string idOrName = "$Alt.SF18Id.Id27";
            string urlParameters = "$expand=none";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            string json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetAsync(
                idOrName,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetByAlternateKeyAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateKeyAsync_AltKey_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetByAlternateKeyAsync(
                    altKey,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateKeyAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = "27";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetByAlternateKeyAsync(
                altKey,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateKeyAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altKey = "27";
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetByAlternateKeyAsync(
                altKey,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetByAlternateIdAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateIdAsync_AltId_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altId = null;
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetByAlternateIdAsync(
                    altId,
                    urlParameters,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByAlternateIdAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string altId = "27";
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetByAlternateIdAsync(
                altId,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetByCustomUrlAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByCustomUrlAsync_urlPart_null()
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

        [TestMethod]
        public async Task EntityClientAsync_GetByCustomUrlAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlPart = "some/service";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupServiceUrl();
            var json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetByCustomUrlAsync(
                urlPart,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region CallByCustomUrlAsync
        [TestMethod]
        public async Task EntityClientAsync_CallByCustomUrlAsync_UrlPart_null()
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
            _MockSetupHelper.SetupServiceUrl();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.CallByCustomUrlAsync(
                urlPart,
                httpMethod,
                content,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetObjectByCustomUrlAsync
        [TestMethod]
        public async Task EntityClientAsync_GetObjectByCustomUrlAsync_urlPart_null()
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
            _MockSetupHelper.SetupServiceUrl();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.GetObjectByCustomUrlAsync(
                urlPart,
                httpMethod,
                content,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetByQueryParametersAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByQueryParametersAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string queryParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetByQueryParametersAsync(
                queryParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetByIdsAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByIdsAsync_Ids_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<string> ids = null;
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
        public async Task EntityClientAsync_GetByIdsAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<string> ids = new[] { "112", "113" };
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<IEnumerable<string>>();

            // Act
            var result = await entityClientAsync.GetByIdsAsync(
                ids,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByIdsAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            IEnumerable<string> ids = new[] { "112", "113" };
            string urlParameters = "$expand=Entity2";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<IEnumerable<string>>();

            // Act
            var result = await entityClientAsync.GetByIdsAsync(
                ids,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetByPropertyValuesAsync
        [TestMethod]
        public async Task EntityClientAsync_GetByPropertyValuesAsync_property_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = null;
            IEnumerable<string> values = null;
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
            IEnumerable<string> values = new[] { "", null };
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<IEnumerable<string>>();

            // Act
            var result = await entityClientAsync.GetByPropertyValuesAsync(
                property,
                values,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetByPropertyValuesAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = "Name";
            IEnumerable<string> values = new[] { "", null };
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<IEnumerable<string>>();

            // Act
            var result = await entityClientAsync.GetByPropertyValuesAsync(
                property,
                values,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
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
            string id = "27";
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
            string id = "27";
            string property = "Name";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunMock();

            // Act
            var result = await entityClientAsync.GetPropertyAsync(
                id,
                property,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region GetMetadataAsync
        [TestMethod]
        public async Task EntityClientAsync_GetMetadataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            _MockSetupHelper.SetupServiceUrl();
            CsdlEntity csdl = _MockSetupHelper.SetupRunAndDeserializeGeneric<CsdlEntity>();

            // Act
            var result = await entityClientAsync.GetMetadataAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(csdl, result);
        }
        #endregion

        #region PatchAsync
        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            HttpContent content = new JsonContent("{}");
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PatchAsync(
                    id,
                    content,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_content_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            HttpContent content = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PatchAsync(
                    id,
                    content,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            HttpContent content = new JsonContent("{}");
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                content,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            HttpContent content = new JsonContent("{}");
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                content,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_HttpContent_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new PatchedEntity<EntityInt, int>();
            var contentJson = JsonConvert.SerializeObject(obj);
            var httpContent = new JsonContent(contentJson);
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                httpContent,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_Obj_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new PatchedEntity<EntityInt, int>();
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                obj,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_HttpContent_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new PatchedEntity<EntityInt, int>();
            var contentJson = JsonConvert.SerializeObject(obj);
            var httpContent = new JsonContent(contentJson);
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                httpContent,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_ObjectContent_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            object obj = new PatchedEntity<EntityInt, int>();
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PatchAsync(
                    id,
                    obj,
                    urlParameters,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_ObjectContent_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = null;
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PatchAsync(
                    id,
                    obj,
                    urlParameters,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PatchAsync_obj_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new PatchedEntity<EntityInt, int>();
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.PatchAsync(
                id,
                obj,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region PostAsync
        [TestMethod]
        public async Task EntityClientAsync_PostAsync_Content_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            HttpContent content = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PostAsync(
                    content,
                    forwardExceptions);
            });
        }


        [TestMethod]
        public async Task EntityClientAsync_PostAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            HttpContent content = new JsonContent("{}");
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PostAsync(
                content,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PostAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            HttpContent content = new JsonContent("{}");
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PostAsync(
                content,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PostAsync_obj_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            object obj = new[] { new EntityInt { Id = 10, Name = "E10" } };
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.PostAsync(
                obj,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PostAsync_obj_UrlParams_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            object content = null;
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PostAsync(
                    content,
                    urlParameters,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PostAsync_obj_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            object obj = new[] { new EntityInt { Id = 10, Name = "E10" } };
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.PostAsync(
                obj,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }
        #endregion

        #region PutAsync
        [TestMethod]
        public async Task EntityClientAsync_PutAsync_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            HttpContent content = new JsonContent("{}");
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PutAsync(
                    id,
                    content,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_content_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            HttpContent content = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PutAsync(
                    id,
                    content,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            HttpContent content = new JsonContent("{}");
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                content,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            HttpContent content = new JsonContent("{}");
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                content,
                urlParameters,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_obj_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new[] { new EntityInt { Id = 10, Name = "E10" } };
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                obj);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_HttpContent_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new[] { new EntityInt { Id = 10, Name = "E10" } };
            var contentJson = JsonConvert.SerializeObject(obj);
            var httpContent = new JsonContent(contentJson);
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                httpContent);

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_obj_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new[] { new EntityInt { Id = 10, Name = "E10" } };
            var contentJson = JsonConvert.SerializeObject(obj);
            var httpContent = new JsonContent(contentJson);
            string urlParameters = null;
            _MockSetupHelper.SetupEntityClientSettings();
            var json = _MockSetupHelper.SetupRunHttpContentMock();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                httpContent,
                urlParameters,
                true
                );

            // Assert
            Assert.AreEqual(json, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_ObjContent_UrlParams_Id_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            object content = new object();
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.PutAsync(
                    id,
                    content,
                    urlParameters,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_ObjContent_UrlParams_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object content = null;
            string urlParameters = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var result = await entityClientAsync.PutAsync(
                    id,
                    content,
                    urlParameters,
                    forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_PutAsync_ObjContent_UrlParams_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            object obj = new[] { new EntityInt { Id = 10, Name = "E10" } };
             string urlParameters = null;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.PutAsync(
                id,
                obj,
                urlParameters,
                true
                );

            // Assert
            Assert.AreEqual(json, result);
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
            string value = null;
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
            string value = null;
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
            string id = "27";
            string property = "Name";
            string value = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();
            var json = _MockSetupHelper.SetupRunTContentMock<object>();

            // Act
            var result = await entityClientAsync.UpdatePropertyAsync(
                id,
                property,
                value,
                forwardExceptions);

            // Assert
            Assert.AreEqual(json, result);
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
            _MockSetupHelper.SetupRunAndDeserializeGeneric(10);

            // Act
            var result = await entityClientAsync.GetCountAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_GetCountAsync_Url_Params_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string urlParameters = null;
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupRunAndDeserializeGeneric(10);

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

        #region 
        [TestMethod]
        public async Task EntityClientAsync_GenerateRepositoryAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            _MockSetupHelper.SetupServiceUrl();
            var repoGenResult = _MockSetupHelper.SetupRunAndDeserializeGeneric<RepositoryGenerationResult>();

            // Act
            var result = await entityClientAsync.GenerateRepositoryAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(repoGenResult, result);
        }
        #endregion

        #region InsertSeedDataAsync
        [TestMethod]
        public async Task EntityClientAsync_InsertSeedDataAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            bool forwardExceptions = false;
            _MockSetupHelper.SetupServiceUrl();
            var repositorySeedResult = _MockSetupHelper.SetupRunAndDeserializeGeneric<RepositorySeedResult>();

            // Act
            var result = await entityClientAsync.InsertSeedDataAsync(
                forwardExceptions);

            // Assert
            Assert.AreEqual(repositorySeedResult, result);
        }
        #endregion

        #region DeleteExtensionAsync

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAllAsync_id_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            string extensionEntity = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.DeleteAllExtensionsAsync<int>(id, extensionEntity, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAllAsync_extensionEntity_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "1";
            string extensionEntity = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.DeleteAllExtensionsAsync<int>(id, extensionEntity, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAllAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            string extensionEntity = "Addendum";

            bool forwardExceptions = false;

            Dictionary<int, bool> extensionEntityIds = new Dictionary<int, bool>();
            extensionEntityIds.Add(190, true);
            extensionEntityIds.Add(201, true);

            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<Dictionary<int, bool>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                  .ReturnsAsync(extensionEntityIds);

            _MockSetupHelper.SetupEntityClientSettings();

            // Act
            var result = await entityClientAsync.DeleteAllExtensionsAsync<int>(id, extensionEntity, forwardExceptions);

            // Assert
            Assert.AreEqual(extensionEntityIds, result);
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAsync_id_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = null;
            string extensionEntity = null;
            var extentionEntityIds = new List<string> { "190", "201" };
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.DeleteExtensionsAsync<int>(id, extensionEntity, extentionEntityIds, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAsync_extensionEntity_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "1";
            string extensionEntity = null;
            var extentionEntityIds = new List<string> { "190", "201" };
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.DeleteExtensionsAsync<int>(id, extensionEntity, extentionEntityIds, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAsync_extensionEntityIds_null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string id = "1";
            string extensionEntity = null;
            var extentionEntityIds = new List<string> { "190", "201" };
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.DeleteExtensionsAsync<int>(id, extensionEntity, extentionEntityIds, forwardExceptions);
            });
        }

        [TestMethod]
        public async Task EntityClientAsync_DeleteExtensionAsync_WithIds_Test()
        {
            var entityClientAsync = CreateEntityClientAsync();
            string id = "27";
            string extensionEntity = "Addendum";
            var extentionEntityIds = new List<string> { "190", "201" };
            bool forwardExceptions = false;

            Dictionary<int, bool> extensionEntityIds = new Dictionary<int, bool>();
            extensionEntityIds.Add(190, true);
            extensionEntityIds.Add(201, true);
            extensionEntityIds.Add(234, false);

            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<object, Dictionary<int, bool>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<IEnumerable<string>>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                  .ReturnsAsync(extensionEntityIds);

            _MockSetupHelper.SetupEntityClientSettings();
            _MockSetupHelper.SetupJsonSerializerSettings();

            // Act
            var result = await entityClientAsync.DeleteExtensionsAsync<int>(id, extensionEntity, extentionEntityIds, forwardExceptions);

            // Assert
            Assert.AreEqual(extensionEntityIds, result);
        }

        #endregion

        #region GetDistinctPropertyValuesAsync
        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetDistinctPropertyValuesAsync_Property_Null()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = null;
            bool forwardExceptions = false;

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await entityClientAsync.GetDistinctPropertyValuesAsync(
                    property,
                    forwardExceptions);
            });
        }


        [TestMethod]
        public async Task ExtensionEntityClientAsync_GetDistinctPropertyValuesAsync_Test()
        {
            // Arrange
            var entityClientAsync = CreateEntityClientAsync();
            string property = "Property";
            bool forwardExceptions = false;
            _MockSetupHelper.SetupEntityClientSettings();
            List<string> distinctProperties = _MockSetupHelper.SetupRunAndDeserializeList();

            // Act
            var result = await entityClientAsync.GetDistinctPropertyValuesAsync(
                property,
                forwardExceptions);

            // Assert
            Assert.AreEqual(distinctProperties, result);
        }
        #endregion
    }
}
