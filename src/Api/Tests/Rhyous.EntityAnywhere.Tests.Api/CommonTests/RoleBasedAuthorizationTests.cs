using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using Rhyous.StringLibrary;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{

    [TestClass]
    public class RoleBasedAuthorizationTests
    {
        public static IEntityClientConfig EntityClientConfig;

        private static IAdminHttpClientRunner _AdminHttpClientRunner;
        private static IAdminHeaders _AdminHeaders;
        private static IToken _UserToken;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) => TaskRunner.RunSynchonously(ClassInitializeAsync, context);

        private static async Task ClassInitializeAsync(TestContext context)
        {
            EntityClientConfig = new TestConfiguration(context);
            _AdminHeaders = new AdminHeaders(EntityClientConfig);
            _AdminHttpClientRunner = new AdminHttpClientRunner(HttpClientFactory.Instance, _AdminHeaders);

            var userSettings = new EntityClientConnectionSettings<User>(EntityClientConfig);
            var userClient = new AdminEntityClientAsync<User, long>(userSettings, _AdminHttpClientRunner);
            var password = CryptoRandomString.GetCryptoRandomAlphaNumericString(12);
            var rand = CryptoRandomString.GetCryptoRandomAlphaNumericString(4);
            var user = new User { Username = $"TestUser{DateTimeOffset.Now.ToUnixTimeMilliseconds()}{rand}", Password = password, Enabled = true, IsHashed = false };
            var odataPostedUser = (await userClient.PostAsync(new[] { user })).FirstOrDefault();

            var userRoleSettings = new EntityClientConnectionSettings<UserRole>(EntityClientConfig);
            var userRoleClient = new AdminEntityClientAsync<UserRole, int>(userRoleSettings, _AdminHttpClientRunner);
            var userRole = new UserRole { Name = $"TestRole{DateTimeOffset.Now.ToUnixTimeMilliseconds()}{rand}", LandingPageId = 1 };
            var odataPostedUserRole = (await userRoleClient.PostAsync(new[] { userRole })).FirstOrDefault();

            var entitySettings = new EntityClientConnectionSettings<Entity>(EntityClientConfig);
            var entityClient = new AdminEntityClientAsync<Entity, int>(entitySettings, _AdminHttpClientRunner);
            var odataEntity = await entityClient.GetAsync("$Alt.$Key.Addendum");

            var userRoleEntityMapSettings = new EntityClientConnectionSettings<UserRoleEntityMap>(EntityClientConfig);
            var userRoleEntityMapClient = new AdminEntityClientAsync<UserRoleEntityMap, long>(userRoleEntityMapSettings, _AdminHttpClientRunner);
            var userRoleEntityMap = new UserRoleEntityMap { EntityId = odataEntity.Id, UserRoleId = odataPostedUserRole.Id };
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
        public async Task EAF_UserAssignedRole_HasRoleHasSku_Access_CanGetSku_Test()
        {
            // Arrange
            var addendumSettings = new EntityClientConnectionSettings<Addendum>(EntityClientConfig);
            var headers = new Headers { Collection = new NameValueCollection { { "Token", _UserToken.Text } } };
            var httpClientRunner = new HttpClientRunner(HttpClientFactory.Instance, headers);
            var addendumClient = new EntityClientAsync<Addendum, long>(addendumSettings, httpClientRunner);

            // Act
            var actual = await addendumClient.GetByQueryParametersAsync("$top=1&$Expand=none");

            // Assert
            Assert.AreEqual(1, actual.Count);
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