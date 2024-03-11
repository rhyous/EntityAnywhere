using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    [TestClass]
    public class UserRoleMembershipMembershipTests
    {
        public const string DateTimeFormat = "yyMMHHmmssffff";
        public static IEntityClientConfig EntityClientConfig;
        public IAdminHttpClientRunner _AdminHttpClientRunner;
        private IAdminHeaders _AdminHeaders;
        public EntityClientConnectionSettings<UserRoleMembership> _UserRoleMembershipSettings;
        public IMappingEntitySettings<UserRoleMembership> _MappingEntitySettings;

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
            _UserRoleMembershipSettings = new EntityClientConnectionSettings<UserRoleMembership>(EntityClientConfig);
            _MappingEntitySettings = new MappingEntitySettings<UserRoleMembership>();
        }

        [TestMethod]
        [TestCategory("Ready")]
        public async Task EAF_UserRoleMembership_GetByMappingsAsync_ApiTest()
        {
            // Arrange
            var client = new AdminMappingEntityClientAsync<UserRoleMembership, long, int, long>(_UserRoleMembershipSettings, _MappingEntitySettings, _AdminHttpClientRunner);
            var mappings = new List<UserRoleMembership> { new UserRoleMembership { UserRoleId = 1, UserId = 1 } };

            // Act
            var odataObjectUserRoleMemberships = await client.GetByMappingsAsync(mappings);

            // Assert
            Assert.AreEqual(1, odataObjectUserRoleMemberships.Count);
        }
    }
}