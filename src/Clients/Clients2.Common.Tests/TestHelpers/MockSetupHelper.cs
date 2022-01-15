using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using System.Net.Http;
using TEntity = Rhyous.EntityAnywhere.Clients2.Common.Tests.EntityInt;
using TId = System.Int32;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    internal class MockSetupHelper
    {
        private readonly Mock<IEntityClientConnectionSettings> _MockEntityClientSettings;
        private readonly Mock<IEntityClientConnectionSettings<TEntity>> _MockEntityClientSettingsGeneric;
        private readonly Mock<IHttpClientRunner> _MockHttpClientRunner;

        public MockSetupHelper(Mock<IEntityClientConnectionSettings<TEntity>> mockEntityClientSettings,
                                Mock<IHttpClientRunner> mockHttpClientRunner)
        {
            _MockEntityClientSettingsGeneric = mockEntityClientSettings;
            _MockHttpClientRunner = mockHttpClientRunner;
        }

        public MockSetupHelper(Mock<IEntityClientConnectionSettings> mockEntityClientSettings,
                        Mock<IHttpClientRunner> mockHttpClientRunner)
        {
            _MockEntityClientSettings = mockEntityClientSettings;
            _MockHttpClientRunner = mockHttpClientRunner;
        }

        internal void SetupEntityClientSettings()
        {
            SetupServiceUrl();
            SetupEntityNamePluralized();
        }

        internal void SetupServiceUrl()
        {
            _MockEntityClientSettings?.Setup(m => m.ServiceUrl)
                                     .Returns("https://eaf.domain.tld/ThisService");
            _MockEntityClientSettingsGeneric?.Setup(m => m.ServiceUrl)
                                     .Returns("https://eaf.domain.tld/ThisService");
        }

        internal void SetupEntityNamePluralized()
        {
            _MockEntityClientSettings?.Setup(m => m.EntityNamePluralized)
                                     .Returns("EntityInts");
            _MockEntityClientSettingsGeneric?.Setup(m => m.EntityNamePluralized)
                                     .Returns("EntityInts");
        }

        internal void SetupJsonSerializerSettings(JsonSerializerSettings settings = null)
        {
            _MockEntityClientSettings?.Setup(m => m.JsonSerializerSettings)
                                     .Returns(settings ?? new JsonSerializerSettings());
            _MockEntityClientSettingsGeneric?.Setup(m => m.JsonSerializerSettings)
                                     .Returns(settings ?? new JsonSerializerSettings());
        }

        internal void SetupDeleteMocks(bool succeeds)
        {
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<bool>(It.IsAny<HttpMethod>(),
                                                           It.IsAny<string>(),
                                                           It.IsAny<bool>()))
                                 .ReturnsAsync(succeeds);
            SetupEntityClientSettings();
        }

        internal string SetupRunMock()
        {
            var json = "{}";
            _MockHttpClientRunner.Setup(m => m.Run(It.IsAny<HttpMethod>(),
                                                   It.IsAny<string>(),
                                                   It.IsAny<bool>()))
                                 .ReturnsAsync(json);
            return json;
        }

        internal string SetupRunHttpContentMock()
        {
            var json = "{}";
            _MockHttpClientRunner.Setup(m => m.Run(It.IsAny<HttpMethod>(),
                                                   It.IsAny<string>(),
                                                   It.IsAny<HttpContent>(),
                                                   It.IsAny<bool>()))
                                 .ReturnsAsync(json);
            return json;
        }

        internal string SetupRunTContentMock<TContent>()
        {
            var json = "{}";
            _MockHttpClientRunner.Setup(m => m.Run(It.IsAny<HttpMethod>(),
                                                   It.IsAny<string>(),
                                                   It.IsAny<object>(),
                                                   It.IsAny<JsonSerializerSettings>(),
                                                   It.IsAny<bool>()))
                                 .ReturnsAsync(json);
            return json;
        }

        internal OdataObjectCollection<TEntity, TId> SetupRunAndDeserializeCollectionMock()
        {
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntities = new[] { entity }.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<OdataObjectCollection<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);
            return odataEntities;
        }

        internal OdataObject<TEntity, int> SetupRunAndDeserializeSingleMock()
        {
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<OdataObject<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntity);
            return odataEntity;
        }

        internal OdataObjectCollection<TEntity, TId> SetupRunAndDeserializeCollectionWithObjectContent()
        {
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntities = new[] { entity }.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<object, OdataObjectCollection<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<object>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);
            return odataEntities;
        }

        internal OdataObjectCollection<TEntity, TId> SetupRunAndDeserializeCollectionWithTContent<TContent>()
        {
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntities = new[] { entity }.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<TContent, OdataObjectCollection<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<TContent>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);
            return odataEntities;
        }

        internal OdataObject<TEntity, TId> SetupRunAndDeserializeSingleWithObjectContent()
        {
            var entity = new TEntity { Id = 11, Name = "E1" };
            var odataEntity = entity.AsOdata<TEntity, TId>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<object, OdataObject<TEntity, TId>>(
                                               It.IsAny<HttpMethod>(),
                                               It.IsAny<string>(),
                                               It.IsAny<object>(),
                                               It.IsAny<JsonSerializerSettings>(),
                                               It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntity);
            return odataEntity;
        }

        internal TResult SetupRunAndDeserializeGeneric<TResult>(TResult t1 = default(TResult))
            where TResult : new()
        {
            t1 = t1 ?? new TResult();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<TResult>(
                                   It.IsAny<HttpMethod>(),
                                   It.IsAny<string>(),
                                   It.IsAny<bool>()))
                     .ReturnsAsync(t1);
            return t1;
        }
    }
}
