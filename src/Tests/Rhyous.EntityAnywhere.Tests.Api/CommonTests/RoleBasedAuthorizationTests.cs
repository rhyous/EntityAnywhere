using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{

    [TestClass]
    public class RoleBasedAuthorizationTests
    {
        private static DynamicEntityJsonProvider _DynamicEntityJsonProvider;

        public static IEntityClientConfig EntityClientConfig;

        private IAdminHttpClientRunner _AdminHttpClientRunner;
        private IAdminHeaders _AdminHeaders;
        private IToken _UserToken;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var entityTypeJsonProviderDictionary = new EntityTypeJsonProviderDictionary();
            _DynamicEntityJsonProvider = new DynamicEntityJsonProvider(entityTypeJsonProviderDictionary);

            EntityClientConfig = new TestConfiguration(context);
        }


        [TestInitialize]
        public void TestInitialize() => TaskRunner.RunSynchonously(TestInitializeAsync);

        private async Task TestInitializeAsync()
        {
            _AdminHeaders = new AdminHeaders(EntityClientConfig);
            _AdminHttpClientRunner = new AdminHttpClientRunner(HttpClientFactory.Instance, _AdminHeaders);

            const int addendumEntityId = 1;
            var userSettings = new EntityClientConnectionSettings<User>(EntityClientConfig);
            var userClient = new AdminEntityClientAsync<User, long>(userSettings, _AdminHttpClientRunner);
            var password = CryptoRandomString.GetCryptoRandomAlphaNumericString(12);
            var user = new User { Username = $"TestUser{DateTimeOffset.Now.ToUnixTimeMilliseconds()}", Password = password, Enabled = true, IsHashed = false };
            var odataPostedUser = (await userClient.PostAsync(new[] { user })).FirstOrDefault();

            var userRoleSettings = new EntityClientConnectionSettings<UserRole>(EntityClientConfig);
            var userRoleClient = new AdminEntityClientAsync<UserRole, int>(userRoleSettings, _AdminHttpClientRunner);
            var userRole = new UserRole { Name = $"TestRole{DateTimeOffset.Now.ToUnixTimeMilliseconds()}" };
            var odataPostedUserRole = (await userRoleClient.PostAsync(new[] { userRole })).FirstOrDefault();

            var userRoleEntityMapSettings = new EntityClientConnectionSettings<UserRoleEntityMap>(EntityClientConfig);
            var userRoleEntityMapClient = new AdminEntityClientAsync<UserRoleEntityMap, long>(userRoleEntityMapSettings, _AdminHttpClientRunner);
            var userRoleEntityMap = new UserRoleEntityMap { EntityId = addendumEntityId, UserRoleId = odataPostedUserRole.Id };
            var odataPostedUserRoleEntityMap = await userRoleEntityMapClient.PostAsync(new[] { userRoleEntityMap });

            var userRoleMembershipSettings = new EntityClientConnectionSettings<UserRoleMembership>(EntityClientConfig);
            var userRoleMembershipClient = new AdminEntityClientAsync<UserRoleMembership, long>(userRoleMembershipSettings, _AdminHttpClientRunner);
            var userRoleMembership = new UserRoleMembership { UserId = odataPostedUser.Id, UserRoleId = odataPostedUserRole.Id };
            var odataPostedUserRoleMembership = (await userRoleMembershipClient.PostAsync(new[] { userRoleMembership })).FirstOrDefault();

            var serviceUrl = EntityClientConfig.GetServiceUrl("AuthenticationService");
            var authClient = new AuthenticationClientFactory(null).Create(serviceUrl);

            _UserToken = await authClient.AuthenticateAsync(user.Username, user.Password);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_UserAssignedRole_HasRoleHasAddendum_Access_CanGetAddendum_Test()
        {
            // Arrange
            var addendumSettings = new EntityClientConnectionSettings<Addendum>(EntityClientConfig);
            var headers = new Headers { Collection = new NameValueCollection { { "Token", _UserToken.Text } } };
            var httpClientRunner = new HttpClientRunner(HttpClientFactory.Instance, headers);
            var addendumClient = new EntityClientAsync<Addendum, long>(addendumSettings, httpClientRunner);

            // Act
            var addendums = await addendumClient.GetByQueryParametersAsync("$top=1&$Expand=none");

            // Assert
            Assert.AreEqual(1, addendums.Count);
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_AuthorizationWebService_UserRole_Test()
        {
            // Arrange
            var serviceUrl = EntityClientConfig.GetServiceUrl("AuthorizationService");
            var authorizationClient = new AuthorizationClientFactory(null).Create(_UserToken.Text, serviceUrl);

            // Act
            var userRoleEntityData = await authorizationClient.GetRoleDataAsync();

            // Assert
            Assert.AreEqual(1, userRoleEntityData.Count);
            Assert.AreEqual("Addendum", userRoleEntityData["Addendum"].Entity);
            Assert.IsTrue(userRoleEntityData["Addendum"].Permissions.Contains("Admin"));
        }
    }
}