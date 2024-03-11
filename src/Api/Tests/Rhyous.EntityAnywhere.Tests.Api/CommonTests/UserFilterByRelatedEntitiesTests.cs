using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Rhyous.EntityAnywhere.Clients2.Common;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [TestClass]
    public class UserFilterByRelatedEntitiesTests
    {
        public const string DateTimeFormat = "yyMMHHmmssffff";
        public static IEntityClientConfig EntityClientConfig;
        public static IAdminHttpClientRunner _AdminHttpClientRunner;
        private static IAdminHeaders _AdminHeaders;
        private static EntityClientConnectionSettings _UserSettings;
        private static EntityClientConnectionSettings _AlernateIdSettings;
        private static EntityClientAsync _UserClient;
        private static EntityClientAsync _AlternateIdClient;
        private static string _UserIdStr;
        private static string _AltIdStr;
        private static string _Prop = "PropA";
        private static string _Value;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) => TaskRunner.RunSynchonously(ClassInitializeAsync, context);
        public static async Task ClassInitializeAsync(TestContext context)
        {
            EntityClientConfig = new TestConfiguration(context);
            _AdminHeaders = new AdminHeaders(EntityClientConfig);
            _AdminHttpClientRunner = new AdminHttpClientRunner(HttpClientFactory.Instance, _AdminHeaders);
            _UserSettings = new EntityClientConnectionSettings("User", EntityClientConfig);
            _AlernateIdSettings = new EntityClientConnectionSettings("AlternateId", EntityClientConfig);

            // Create Clients
            _UserClient = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            _AlternateIdClient = new EntityClientAsync(_AlernateIdSettings, _AdminHttpClientRunner);

            // Post user
            var dateString = DateTime.Now.ToString(DateTimeFormat);
            var username = $"ApiTestUser1_{dateString}";
            var user1Json = $"{{\"Username\":\"{username}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var userPostJson = string.Format("[{0}]", user1Json);
            var userPostJsonContent = new JsonContent(userPostJson);
            var postedUsers = await _UserClient.PostAsync(userPostJsonContent);
            var postedUsersJObj = JObject.Parse(postedUsers);
            _UserIdStr = postedUsersJObj["Entities"][0]["Id"].ToString();

            // Post extension AlternateIds
            _Value = $"ValueA{dateString}";
            var altIdJson = $"{{\"Entity\":\"User\",\"EntityId\":\"{_UserIdStr}\",\"Property\":\"{_Prop}\",\"Value\":\"{_Value}\"}}";
            var altIdPostJson = string.Format("[{0}]", altIdJson);
            var altIdPostJsonContent = new JsonContent(altIdPostJson);
            var postedAltIds = await _AlternateIdClient.PostAsync(altIdPostJsonContent);
            var postedAltIdsJObj = JObject.Parse(postedAltIds);
            _AltIdStr = postedAltIdsJObj["Entities"][0]["Id"].ToString();
            _AlternateIdClient = new EntityClientAsync(_AlernateIdSettings, _AdminHttpClientRunner);
        }

        [ClassCleanup]
        public static void ClassCleanup() => TaskRunner.RunSynchonously(ClassCleanupAsync);
        public static async Task ClassCleanupAsync()
        {
            var deletedAltId = await _AlternateIdClient.DeleteAsync(_AltIdStr);
            var deletedUser = await _UserClient.DeleteAsync(_UserIdStr);
            Assert.IsTrue(deletedAltId);
            Assert.IsTrue(deletedUser);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_FilterByRelatedEntity_Equals_ApiTest()
        {
            // Arrange - Done in TestInitializeAsync
            var urlFilter = $"$Filter=AlternateId.{_Prop} eq '{_Value}'";

            // Act
            var resultJson = await _UserClient.GetByQueryParametersAsync(urlFilter);

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"];
            Assert.IsTrue(count > 0);
            Assert.IsTrue(resultJobj["Entities"].Any(u => u["Id"].ToString() == _UserIdStr));
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_FilterByRelatedEntity_Contains_ApiTest()
        {
            // Arrange - Done in TestInitializeAsync
            var urlFilter = $"$Filter=Contains(AlternateId.{_Prop}, '{_Value}')";

            // Act
            var resultJson = await _UserClient.GetByQueryParametersAsync(urlFilter);

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"];
            Assert.IsTrue(count > 0);
            Assert.IsTrue(resultJobj["Entities"].Any(u => u["Id"].ToString() == _UserIdStr));
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_FilterByRelatedEntity_InArray_ApiTest()
        {
            // Arrange - Done in TestInitializeAsync
            var urlFilter = $"$Filter=AlternateId.{_Prop} in ('{_Value}')";

            // Act
            var resultJson = await _UserClient.GetByQueryParametersAsync(urlFilter);

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"];
            Assert.IsTrue(count > 0);
            Assert.IsTrue(resultJobj["Entities"].Any(u => u["Id"].ToString() == _UserIdStr));
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_FilterByRelatedEntensionEntity_Equals_ApiTest()
        {
            // Arrange - Done in TestInitializeAsync
            var urlFilter = $"$Filter=AlternateId.{_Prop} eq '{_Value}'";

            // Act
            var resultJson = await _UserClient.GetByQueryParametersAsync(urlFilter);

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"].ToString().To<int>();
            Assert.AreEqual(1, count);
            var foundUserId = resultJobj["Entities"][0]["Id"].ToString();
            Assert.AreEqual(_UserIdStr, foundUserId);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_FilterByRelatedEntensionEntity_Contains_ApiTest()
        {
            // Arrange
            var urlFilter = $"$Filter=Contains(AlternateId.{_Prop}, '{_Value}')";

            // Act
            var resultJson = await _UserClient.GetByQueryParametersAsync(urlFilter);

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"].ToString().To<int>();
            Assert.AreEqual(1, count);
            var foundUserId = resultJobj["Entities"][0]["Id"].ToString();
            Assert.AreEqual(_UserIdStr, foundUserId);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_FilterByRelatedEntensionEntity_InArray_ApiTest()
        {
            // Arrange
            var urlFilter = $"$Filter=AlternateId.{_Prop} in ('{_Value}')";

            // Act
            var resultJson = await _UserClient.GetByQueryParametersAsync(urlFilter);

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"].ToString().To<int>();
            Assert.AreEqual(1, count);
            var foundUserId = resultJobj["Entities"][0]["Id"].ToString();
            Assert.AreEqual(_UserIdStr, foundUserId);
        }
    }
}