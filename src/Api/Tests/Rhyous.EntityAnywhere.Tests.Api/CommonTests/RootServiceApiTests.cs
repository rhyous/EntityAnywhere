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
        private IRootClient _RootClient;

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
            _RootClient = new RootClient(_AdminHttpClientRunner, EntityClientConfig);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task EAF_GeneratorRepository_Test()
        {
            // Arrange
            // Act
            var repositoryGenerationResults = await _RootClient.GenerateRepository();

            // Assert
            Assert.IsNotNull(repositoryGenerationResults);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task EAF_Metadata_Test()
        {
            // Arrange
            // Act
            var csdlDocument = await _RootClient.GetMetadataAsync();

            // Assert
            Assert.IsNotNull(csdlDocument);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task EAF_EntitySettings_Test()
        {
            // Arrange
            // Act
            var entitySettings = await _RootClient.ConfigureEntitySettingsAsync();

            // Assert
            Assert.IsNotNull(entitySettings);
        }

        [TestMethod]
        [TestCategory("Ready")]
        [TestCategory("ProdReady")]
        public async Task EAF_Seed_Test()
        {
            // Arrange
            // Act
            var seedResults = await _RootClient.InsertSeedDataAsync();

            // Assert
            Assert.IsNotNull(seedResults);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_Impersonation_Test()
        {
            // Arrange
            var roleId = "2"; // Customer

            // Create User and UserRoleMembership
            var random = CryptoRandomString.GetCryptoRandomAlphaNumericString(4).CapitalizeFirstLetter();
            var user = new User
            {
                Username = $"ApiTest-Impersonate-{random}",
                Password = CryptoRandomString.GetCryptoRandomAlphaNumericString(10),
                Enabled = true
            };
            var odataUser = await _UserClient.PostAsync(new[] { user });

            var userRoleMembership = new UserRoleMembership
            {
                UserId = odataUser.First().Object.Id,
                UserRoleId = 1
            };
            var odataUserRoleMembership = await _UserRoleMembershipClient.PostAsync(new[] { userRoleMembership });

            // Authenticate
            var baseUrl = EntityClientConfig.GetUrl();
            var authUrl = StringConcat.WithSeparator('/', baseUrl, "AuthenticationService");
            var authClient = new AuthenticationClientFactory(null).Create(authUrl);
            var token = await authClient.AuthenticateAsync(new Credentials { User = user.Username, Password = user.Password, AuthenticationPlugin = "Users" });

            // Request impersonation token
            var impersonateUrl = StringConcat.WithSeparator('/', baseUrl, "Service", "$Impersonate", roleId);
            var request = new HttpRequestMessage(HttpMethod.Get, impersonateUrl);
            request.Headers.Add("Token", token.Text);
            var httpclient = new HttpClient();

            // Act
            var response = await httpclient.SendAsync(request);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var newToken = JsonConvert.DeserializeObject<Token>(json);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(DateTimeOffset.Now.Date, newToken.CreateDate.ToLocalTime().Date);

            var orgClaims = newToken.ClaimDomains.First(d => d.Subject == "Organization").Claims;
            Assert.AreEqual("true", orgClaims.First(c => c.Name == "Impersonation").Value);

            var userRoleClaims = newToken.ClaimDomains.First(d => d.Subject == "UserRole").Claims;
            Assert.AreEqual("Customer", userRoleClaims.First(c => c.Name == "Role").Value);
        }
    }
}