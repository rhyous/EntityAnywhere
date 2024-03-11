using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.StringLibrary;
using Rhyous.UnitTesting;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Rhyous.EntityAnywhere.Clients2.Common;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [TestClass]
    public class UserTests
    {
        public const string DateTimeFormat = "yyMMHHmmssffff";
        public static IEntityClientConfig EntityClientConfig;
        public IAdminHttpClientRunner _AdminHttpClientRunner;
        private IAdminHeaders _AdminHeaders;
        public EntityClientConnectionSettings _UserSettings;
        public EntityClientConnectionSettings _AlernateIdSettings;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            EntityClientConfig = new TestConfiguration(context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _AdminHeaders = new AdminHeaders(EntityClientConfig);
            _AdminHttpClientRunner = new AdminHttpClientRunner(HttpClientFactory.Instance, _AdminHeaders);
            _UserSettings = new EntityClientConnectionSettings("User", EntityClientConfig);
            _AlernateIdSettings = new EntityClientConnectionSettings("AlternateId", EntityClientConfig);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GenerateRepository_ApiTest()
        {
            // Arrange
            var client = new AdminEntityClientAsync(_UserSettings, _AdminHttpClientRunner);

            // Act
            var json = await client.GetByCustomUrlAsync("$Generate");

            // Assert
            Assert.AreEqual("{\"Name\":\"User\",\"RepositoryReady\":true}", json);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_Metadata_ApiTest()
        {
            // Arrange
            var client = new AdminEntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userMetadata = System.IO.File.ReadAllText(@"Data/UserMetadata.json");

            // Act
            var json = await client.GetByCustomUrlAsync("$Metadata");

            // Assert
            Assert.AreEqual(userMetadata, json);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GET_GetByQueryParametersAsync_Top1_ApiTest()
        {
            // Arrange
            var client = new AdminEntityClientAsync(_UserSettings, _AdminHttpClientRunner);

            // Act
            var json = await client.GetByQueryParametersAsync("$top=1");

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.IsTrue(count == 0 || count == 1);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GetCountAsync_Top1_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);

            // Act
            var json = await client.GetCountAsync();

            // Assert
            Assert.IsNotNull(json);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GetAsync_ById_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJsonToPost = $"[{{\"Username\":\"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);

            // Act
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var resultJson = await client.GetAsync(id);

            // Assert
            Assert.IsNotNull(resultJson);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [PrimitiveList(typeof(Addendum), typeof(AlternateId), typeof(Note))]
        public async Task EAF_User_PostExtensionAsync_ById_ExtensionEntities_AlsoTestAutoExpandSetting_ApiTest(Type type)
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJsonToPost = $"[{{\"Username\":\"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var propertyValue = new PropertyValue { Property = "PropA", Value = "ValueA" };
            var json = JsonConvert.SerializeObject(propertyValue);

            // Act
            var resultJson = await client.PostExtensionAsync(id, type.Name, json);
            var userResult = await client.GetAsync(id);

            // Assert
            Assert.IsNotNull(resultJson);
            Assert.IsTrue(resultJson.Contains(propertyValue.Property));
            Assert.IsTrue(resultJson.Contains(propertyValue.Value));
            if (type.GetAttribute<ExtensionEntityAttribute>()?.AutoExpand ?? true)
            {
                Assert.IsTrue(userResult.Contains(propertyValue.Property), $"AutoExpand is true for {type}");
                Assert.IsTrue(userResult.Contains(propertyValue.Value), $"AutoExpand is true for {type}");
            }
            else
            {
                Assert.IsFalse(userResult.Contains(propertyValue.Property), $"AutoExpand is true for {type}");
                Assert.IsFalse(userResult.Contains(propertyValue.Value), $"AutoExpand is true for {type}");
            }
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GetAsync_ByAlternateKey_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var username = $"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}";
            var userJsonToPost = $"[{{\"Username\":\"{username}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);

            // Act
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var resultJson = await client.GetAsync(username);

            // Assert
            Assert.IsNotNull(resultJson);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_POST_1_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJson = $"[{{\"Username\":\"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContent = new JsonContent(userJson);

            // Act
            var json = await client.PostAsync(stringContent);

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.IsTrue(count == 1);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_POST_1_Hashed_ApiTest()
        {
            // Arrange
            var settings = new EntityClientConnectionSettings<User>(EntityClientConfig);
            var client = new EntityClientAsync<User, long>(settings, _AdminHttpClientRunner);

            var user = new User
            {
                Username = $"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}",
                Enabled = false,
                ExternalAuth = false,
                IsHashed = true,
                Password = "pw",
                Salt = null
            };

            // Act
            var actualCollection = await client.PostAsync(new List<User> { user });
            Assert.IsNotNull(actualCollection);

            // Assert
            var actual = actualCollection[0].Object;
            Assert.IsNotNull(actual);
            Assert.AreNotEqual(user.Password, actual.Password);
            Assert.IsNotNull(actual.Salt);
            Assert.IsTrue(actual.Salt.Length > 1);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_POST_2_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var user1Json = $"{{\"Username\":\"ApiTestUser1_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var user2Json = $"{{\"Username\":\"ApiTestUser2_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var twoUserJson = string.Format("[{0},{1}]", user1Json, user2Json);
            var stringContent = new JsonContent(twoUserJson);

            // Act
            var json = await client.PostAsync(stringContent);

            // Assert
            Assert.IsNotNull(json);
            var jObj = JObject.Parse(json);
            var count = (int)jObj["Count"];
            Assert.IsTrue(count == 2);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_POST_Duplicate_500Error_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJson = $"[{{\"Username\":\"warehouseone\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":203338,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContent = new JsonContent(userJson);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<ServiceErrorResponseForwarderException>(async () =>
            {
                await client.PostAsync(stringContent);
            }, "The property Username must be unique.\r\nDuplicate User(s) detected: warehouseone");
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_Put_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJsonToPost = $"[{{\"Username\":\"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);
            var userJsonToPut = $"{{\"Username\":\"ApiTestUserX_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pwX\",\"Salt\":null}}";
            var stringContentToPut = new JsonContent(userJsonToPut);

            // Act
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var putResultJson = await client.PutAsync(id, stringContentToPut);

            // Assert
            Assert.IsNotNull(putResultJson);
            var putObj = JObject.Parse(putResultJson);
            Assert.IsTrue(putObj["Object"]["Username"].ToString().StartsWith("ApiTestUserX"));
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_Patch_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJsonToPost = $"[{{\"Username\":\"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);
            var userJsonToPatch = $"{{\"Entity\":{{\"Username\":\"ApiTestUserX_{DateTime.Now.ToString(DateTimeFormat)}\",\"Password\":\"pwX\"}},\"ChangedProperties\":[\"Username\",\"Password\"]}}";
            var stringContentToPatch = new JsonContent(userJsonToPatch);

            // Act
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var patchResultJson = await client.PatchAsync(id, stringContentToPatch);

            // Assert
            Assert.IsNotNull(patchResultJson);
            var putObj = JObject.Parse(patchResultJson);
            Assert.IsTrue(putObj["Object"]["Username"].ToString().StartsWith("ApiTestUserX"));
            Assert.AreEqual("pwX", putObj["Object"]["Password"]);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_PatchMany_ApiTest()
        {
            // Arrange
            var now = DateTime.Now.ToString(DateTimeFormat);
            var username1 = $"ApiTestUser1_{now}";
            var entity1 = new User
            {
                Username = username1,
                Enabled = false,
                ExternalAuth = false,
                IsHashed = false,
                Password = "pw1",
                Salt = null
            };
            var username2 = $"ApiTestUser2_{now}";
            var entity2 = new User
            {
                Username = username2,
                Enabled = false,
                ExternalAuth = false,
                IsHashed = false,
                Password = "pw2",
                Salt = null
            };
            var client = new AdminEntityClientAsync(_UserSettings, _AdminHttpClientRunner);

            var userJsonToPost = JsonConvert.SerializeObject(new[] { entity1, entity2 });
            var stringContentToPost = new JsonContent(userJsonToPost);

            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            entity1.Id = jObj["Entities"][0]["Id"].ToString().To<long>();
            entity1.Username += "x";
            entity1.Password += "x";
            entity2.Id = jObj["Entities"][1]["Id"].ToString().To<long>();
            entity2.Username += "x";
            entity2.Password += "x";

            var patchedEntity1 = new PatchedEntity<User, long> { Entity = entity1, ChangedProperties = new HashSet<string> { "Password" } };
            var patchedEntity2 = new PatchedEntity<User, long> { Entity = entity2, ChangedProperties = new HashSet<string> { "Password" } };
            var patchedEntityCollection = new PatchedEntityCollection<User, long>
            {
                PatchedEntities = new List<PatchedEntity<User, long>> { patchedEntity1, patchedEntity2 },
                ChangedProperties = new HashSet<string> { "Username" }
            };
            var usersPatchJson = JsonConvert.SerializeObject(patchedEntityCollection);
            var stringContentToPatchMany = new JsonContent(usersPatchJson);

            // Act
            var patchResultJson = await client.PatchManyAsync(stringContentToPatchMany);

            // Assert
            Assert.IsNotNull(patchResultJson);
            var putObjs = JObject.Parse(patchResultJson);
            Assert.AreEqual(entity1.Username, putObjs["Entities"][0]["Object"]["Username"].ToString());
            Assert.AreEqual(entity2.Username, putObjs["Entities"][1]["Object"]["Username"].ToString());
            Assert.AreEqual(entity1.Password, putObjs["Entities"][0]["Object"]["Password"].ToString());
            Assert.AreEqual(entity2.Password, putObjs["Entities"][1]["Object"]["Password"].ToString());
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_DeleteAsync_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var userJsonToPost = $"[{{\"Username\":\"ApiTestUser_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost, Encoding.UTF8, "application/json");

            // Act
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var result = await client.DeleteAsync(id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_DeleteManyAsync_ApiTest()
        {
            // Arrange
            var client = new AdminEntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var username1 = $"ApiTestUser1ForDeleteMany_{DateTime.Now.ToString(DateTimeFormat)}";
            var user1 = new User { Username = username1, Password = "password1" };
            var username2 = $"ApiTestUser2ForDeleteMany_{DateTime.Now.ToString(DateTimeFormat)}";
            var user2 = new User { Username = username2, Password = "password2" };
            var userJsonToPost = JsonConvert.SerializeObject(new[] { user1, user2 });
            var stringContentToPost = new JsonContent(userJsonToPost);
            var postedUsers = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUsers);
            var id1 = jObj["Entities"][0]["Id"].ToString().To<int>();
            var id2 = jObj["Entities"][1]["Id"].ToString().To<int>();

            // Act
            var result = await client.DeleteManyAsync(new List<int> { id1, id2 });

            // Assert
            Assert.IsTrue(result[id1]);
            Assert.IsTrue(result[id2]);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GetPropertyAsync_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var datestring = DateTime.Now.ToString(DateTimeFormat);
            var userJsonToPost = $"[{{\"Username\":\"ApiTestUser_{datestring}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);

            // Act
            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();
            var resultJson = await client.GetPropertyAsync(id, "Username");

            // Assert
            Assert.AreEqual($"ApiTestUser_{datestring}", resultJson.Trim('"'));
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_UpdatePropertyAsync_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var datestring = DateTime.Now.ToString(DateTimeFormat);
            var userName = $"ApiTestUser_{datestring}";
            var userJsonToPost = $"[{{\"Username\":\"{userName}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);
            var newUsername = $"ApiTestUser1_{datestring}_x";

            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();

            var property = nameof(User.Username);

            // Act
            var result = await client.UpdatePropertyAsync(id, property, newUsername);

            // Assert
            Assert.AreEqual(newUsername, result.Unquote());
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_UpdatePropertyAsync_CaseInsensitive_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var datestring = DateTime.Now.ToString(DateTimeFormat);
            var userName = $"ApiTestUser_Case_{datestring}";
            var userJsonToPost = $"[{{\"Username\":\"{userName}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}]";
            var stringContentToPost = new JsonContent(userJsonToPost);
            var newUsername = $"{userName}_x";

            var postedUser = await client.PostAsync(stringContentToPost);
            var jObj = JObject.Parse(postedUser);
            var id = jObj["Entities"][0]["Id"].ToString();

            var propertyLowercase = nameof(User.Username).ToLower();

            // Act
            var result = await client.UpdatePropertyAsync(id, propertyLowercase, newUsername);

            // Assert
            Assert.AreEqual(newUsername, result.Unquote());
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GetByPropertyValuesAsync_ApiTest()
        {
            // Arrange
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var username1 = $"ApiTestUser1_{DateTime.Now.ToString(DateTimeFormat)}";
            var user1Json = $"{{\"Username\":\"{username1}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var username2 = $"ApiTestUser2_{DateTime.Now.ToString(DateTimeFormat)}";
            var user2Json = $"{{\"Username\":\"{username2}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var twoUserJson = string.Format("[{0},{1}]", user1Json, user2Json);
            var stringContent = new JsonContent(twoUserJson);

            // Act
            var postedUsers = await client.PostAsync(stringContent);
            var resultJson = await client.GetByPropertyValuesAsync("Username", new[] { username1, username2 });

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"];
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_User_GetByIdsAsync_ApiTest()
        {
            // Arrange
            var client = new EntityClientAsync(_UserSettings, _AdminHttpClientRunner);
            var user1Json = $"{{\"Username\":\"ApiTestUser1_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var user2Json = $"{{\"Username\":\"ApiTestUser2_{DateTime.Now.ToString(DateTimeFormat)}\",\"Enabled\":false,\"ExternalAuth\":false,\"IsHashed\":false,\"OrganizationId\":1,\"Password\":\"pw\",\"Salt\":null}}";
            var twoUserJson = string.Format("[{0},{1}]", user1Json, user2Json);
            var stringContent = new JsonContent(twoUserJson);

            // Act
            var postedUsers = await client.PostAsync(stringContent);
            var jObj = JObject.Parse(postedUsers);
            var id1 = jObj["Entities"][0]["Id"].ToString();
            var id2 = jObj["Entities"][1]["Id"].ToString();
            var resultJson = await client.GetByIdsAsync(new[] { id1, id2 });

            // Assert
            var resultJobj = JObject.Parse(resultJson);
            var count = (int)resultJobj["Count"];
            Assert.AreEqual(2, count);
        }
    }
}