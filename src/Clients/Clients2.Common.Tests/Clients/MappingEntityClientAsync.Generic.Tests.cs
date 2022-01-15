using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Rhyous.Odata;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using TEntity = Rhyous.EntityAnywhere.Clients2.Common.Tests.MappingEntityInt;
using TId = System.Int32;
using TE1Id = System.Int32;
using TE2Id = System.String;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    [TestClass]
    public class MappingEntityClientAsyncTests
    {
        private MockRepository _MockRepository;

        private Mock<IEntityClientConnectionSettings<TEntity>> _MockEntityClientConnectionSettings;
        private Mock<IMappingEntitySettings<TEntity>> _MockMappingEntitySettings;
        private Mock<IHttpClientRunner> _MockHttpClientRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockEntityClientConnectionSettings = _MockRepository.Create<IEntityClientConnectionSettings<TEntity>>();
            _MockMappingEntitySettings = _MockRepository.Create<IMappingEntitySettings<TEntity>>();
            _MockHttpClientRunner = _MockRepository.Create<IHttpClientRunner>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _MockRepository.VerifyAll();
        }

        private MappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id> CreateMappingEntityClientAsync()
        {
            return new MappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>(
                _MockEntityClientConnectionSettings.Object,
                _MockMappingEntitySettings.Object,
                _MockHttpClientRunner.Object);
        }

        #region Constructor tests
        [TestMethod]
        public void EntityClientAsync_Constructor_MappingSettings_Null_Test()
        {

            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new MappingEntityClientAsync<TEntity, TId, TE1Id, TE2Id>(
                                _MockEntityClientConnectionSettings.Object,
                                null,
                                _MockHttpClientRunner.Object
                    );
            });
        }
        #endregion

        [TestMethod]
        public async Task MappingEntityClientAsync_GetByE1IdsAsync_Ids_Null()
        {
            // Arrange
            var mappingEntityClientAsync = CreateMappingEntityClientAsync();
            IEnumerable<int> ids = null;

            _MockMappingEntitySettings.Setup(m => m.Entity1Pluralized)
                                      .Returns($"{nameof(EntityInt)}s");

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await mappingEntityClientAsync.GetByE1IdsAsync(ids);
            });
        }

        [TestMethod]
        public async Task MappingEntityClientAsync_GetByE1IdsAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mappingEntityClientAsync = CreateMappingEntityClientAsync();
            IEnumerable<int> ids = new[] { 27 };
            _MockEntityClientConnectionSettings?.Setup(m => m.ServiceUrl)
              .Returns("https://eaf.domain.tld/ThisService");
            _MockEntityClientConnectionSettings?.Setup(m => m.EntityNamePluralized)
              .Returns($"{nameof(TEntity)}s"); 
            _MockEntityClientConnectionSettings?.Setup(m => m.JsonSerializerSettings)
              .Returns((JsonSerializerSettings)null); 
            _MockMappingEntitySettings.Setup(m => m.Entity1Pluralized)
                                      .Returns($"{nameof(EntityInt)}s");
            var entity = new TEntity { Id = 17, EntityIntId = 27, EntityStringId = "1027" };
            var odataEntities = new[] { entity }.AsOdata<TEntity,int>();
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<List<TE1Id>, OdataObjectCollection<TEntity, TId>>(
                                        It.IsAny<HttpMethod>(),
                                        It.IsAny<string>(),
                                        It.IsAny<List<TE1Id>>(),
                                        It.IsAny<JsonSerializerSettings>(),
                                        It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);

            // Act
            var result = await mappingEntityClientAsync.GetByE1IdsAsync(
                ids);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }

        [TestMethod]
        public async Task MappingEntityClientAsync_GetByE2IdsAsync_Ids_Null()
        {
            // Arrange
            var mappingEntityClientAsync = CreateMappingEntityClientAsync();
            IEnumerable<string> ids = null;

            _MockMappingEntitySettings.Setup(m => m.Entity2Pluralized)
                          .Returns($"{nameof(EntityString)}s");

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                var result = await mappingEntityClientAsync.GetByE2IdsAsync(
                    ids);
            });
        }

        [TestMethod]
        public async Task MappingEntityClientAsync_GetByE2IdsAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mappingEntityClientAsync = CreateMappingEntityClientAsync();
            IEnumerable<string> ids = new[] { "1027" };
            var entity = new TEntity { Id = 17, EntityIntId = 27, EntityStringId = "1027" };
            var odataEntities = new[] { entity }.AsOdata<TEntity, int>();
            _MockEntityClientConnectionSettings?.Setup(m => m.ServiceUrl)
              .Returns("https://eaf.domain.tld/ThisService");
            _MockEntityClientConnectionSettings?.Setup(m => m.EntityNamePluralized)
              .Returns($"{nameof(TEntity)}s");
            _MockEntityClientConnectionSettings?.Setup(m => m.JsonSerializerSettings)
              .Returns((JsonSerializerSettings)null); 
            _MockMappingEntitySettings.Setup(m => m.Entity2Pluralized)
                                      .Returns($"{nameof(EntityString)}s");
            _MockHttpClientRunner.Setup(m => m.RunAndDeserialize<List<TE2Id>, OdataObjectCollection<TEntity, TId>>(
                                        It.IsAny<HttpMethod>(),
                                        It.IsAny<string>(),
                                        It.IsAny<List<TE2Id>>(),
                                        It.IsAny<JsonSerializerSettings>(),
                                        It.IsAny<bool>()))
                                 .ReturnsAsync(odataEntities);

            // Act
            var result = await mappingEntityClientAsync.GetByE2IdsAsync(
                ids);

            // Assert
            Assert.AreEqual(odataEntities, result);
        }
    }
}
