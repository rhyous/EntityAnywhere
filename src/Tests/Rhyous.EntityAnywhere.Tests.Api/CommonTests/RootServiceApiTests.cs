using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{

    [TestClass]
    public class RootServiceApiTests
    {
        public static IEntityClientConfig EntityClientConfig;
        private IAdminHttpClientRunner _AdminHttpClientRunner;
        private IAdminHeaders _AdminHeaders;
        private IAdminEntityClientAsync<User, long> _UserClient;
        private IAdminEntityClientAsync<UserRoleMembership, long> _UserRoleMembershipClient;

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

            var userSettings = new EntityClientConnectionSettings<User>(EntityClientConfig);
            _UserClient = new AdminEntityClientAsync<User, long>(userSettings, _AdminHttpClientRunner);
            var userRoleMembershipSettings = new EntityClientConnectionSettings<UserRoleMembership>(EntityClientConfig);
            _UserRoleMembershipClient = new AdminEntityClientAsync<UserRoleMembership, long>(userRoleMembershipSettings, _AdminHttpClientRunner);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task EAF_EntitySettings_Test()
        {
            // Arrange
            var url = EntityClientConfig.EntityWebHost;
            if (!string.IsNullOrWhiteSpace(EntityClientConfig.EntitySubpath))
                url = StringConcat.WithSeparator('/', url, EntityClientConfig.EntitySubpath);
            url = StringConcat.WithSeparator('/', url, "Service", "$EntitySettings");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(nameof(EntityClientConfig.EntityAdminToken), EntityClientConfig.EntityAdminToken);
            var httpclient = new HttpClient();

            // Act
            var response = await httpclient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(json.StartsWith("{") && json.EndsWith("}"));
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task EAF_Seed_Test()
        {
            // Arrange
            var url = EntityClientConfig.EntityWebHost;
            if (!string.IsNullOrWhiteSpace(EntityClientConfig.EntitySubpath))
                url = StringConcat.WithSeparator('/', url, EntityClientConfig.EntitySubpath);
            url = StringConcat.WithSeparator('/', url, "Service", "$Seed");
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(nameof(EntityClientConfig.EntityAdminToken), EntityClientConfig.EntityAdminToken);
            var httpclient = new HttpClient();

            // Act
            var response = await httpclient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(json.StartsWith("[") && json.EndsWith("]"));
        }
    }
}