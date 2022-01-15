using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{

    [TestClass]
    public class EntityApiTests
    {
        private static DynamicEntityJsonProvider _DynamicEntityJsonProvider;

        public static IEntityClientConfig EntityClientConfig;

        private IAdminHttpClientRunner _AdminHttpClientRunner;
        private IAdminHeaders _AdminHeaders;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var entityTypeJsonProviderDictionary = new EntityTypeJsonProviderDictionary();
            _DynamicEntityJsonProvider = new DynamicEntityJsonProvider(entityTypeJsonProviderDictionary);

            EntityClientConfig = new TestConfiguration(context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _AdminHeaders = new AdminHeaders(EntityClientConfig);
            _AdminHttpClientRunner = new AdminHttpClientRunner(HttpClientFactory.Instance, _AdminHeaders);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        [EntityMetadata(nameof(EntityClientConfig), exclude: "EntitledProduct")]
        public async Task EAF_GET_Top1_Test(KeyValuePair<string, CsdlEntity> kvp)
        {
            // Arrange
            var settings = new EntityClientConnectionSettings(kvp.Key, EntityClientConfig);
            var entity = kvp.Key;
            var client = new AdminEntityClientAsync(settings, _AdminHttpClientRunner);

            // Act
            string json;
            try { json = await client.GetByQueryParametersAsync("$top=1"); }
            catch (Exception e) { throw new Exception(entity, e); }
            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.IsTrue(count == 0 || count == 1);
        }

        /// <summary>This is for testing a single entity manually. Running it in automated builds is redundant with the above method.</summary>
        [TestMethod]
        [TestCategory("Manual")]
        public async Task EAF_GET_Top1_Manual_Test()
        {
            // Arrange
            var entity = "Order";
            var settings = new EntityClientConnectionSettings(entity, EntityClientConfig);
            var client = new EntityClientAsync(settings, _AdminHttpClientRunner);

            // Act
            var json = await client.GetByQueryParametersAsync("$top=1");

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.IsTrue(count == 0 || count == 1);
        }

        [TestMethod]
        public async Task EAF_GET_Top1_single_Test()
        {
            // Arrange
            var entitledProductUsageSettings = new EntityClientConnectionSettings("EntitledProductUsage", EntityClientConfig);
            var client = new AdminEntityClientAsync(entitledProductUsageSettings, _AdminHttpClientRunner);

            // Act
            var json = await client.GetByQueryParametersAsync("$top=1");

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.IsTrue(count == 0 || count == 1);
        }

        [TestMethod]
        [PostEntityMetadata(nameof(EntityClientConfig), exclude: new[]
            {
                "CoreServer", // Why aren't we testing this one?
                "Country", // This isn't in use
                "EntitledProduct", // This is a calculated entity so there is no $top=1
                "EntitledProductUsage", // This has a custom test
                "File", // Why aren't we testing this one?
                "User" // This has a custom test
            })]
        public async Task EAF_POST_Test(KeyValuePair<string, CsdlEntity> kvp, List<KeyValuePair<string, CsdlEntity>> prereqs)
        {
            // Arrange
            var postedPrereqs = new Dictionary<string, OdataObject>();
            foreach (var kvp2 in prereqs)
            {
                var settings2 = new EntityClientConnectionSettings(kvp2.Key, EntityClientConfig);
                var prereqClient = new EntityClientAsync(settings2, _AdminHttpClientRunner);
                var prereqRequestJson = _DynamicEntityJsonProvider.Provide(kvp2.Key, kvp2.Value, postedPrereqs);
                var prereqStringContent = new StringContent(prereqRequestJson);
                var prereqJson = await prereqClient.PostAsync(prereqStringContent);
                var prereqOdata = JsonConvert.DeserializeObject<OdataObjectCollection>(prereqJson);
                var odataObj = prereqOdata.FirstOrDefault();
                postedPrereqs.Add(kvp2.Key, odataObj);
            }

            var requestJson = _DynamicEntityJsonProvider.Provide(kvp.Key, kvp.Value, postedPrereqs);
            HttpContent stringContent = new StringContent(requestJson);
            var settings = new EntityClientConnectionSettings(kvp.Key, EntityClientConfig);
            var client = new EntityClientAsync(settings, _AdminHttpClientRunner);

            // Act
            var json = await client.PostAsync(stringContent);

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public async Task EAF_POST_OneEntity_Test()
        {
            // Arrange
            var postedPrereqs = new Dictionary<string, OdataObject>();
            var entityName = "ResActivation";
            var settings = new EntityClientConnectionSettings(entityName, EntityClientConfig);
            var client = new EntityClientAsync(settings, _AdminHttpClientRunner);
            var csdl = await client.GetMetadataAsync();
            var requestJson = _DynamicEntityJsonProvider.Provide(entityName, csdl, postedPrereqs);
            HttpContent stringContent = new StringContent(requestJson);

            // Act
            var json = await client.PostAsync(stringContent);

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.AreEqual(1, count);
        }
    }
}