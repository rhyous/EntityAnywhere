using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class RandomUserGeneratorService
    {
        private IAdminEntityClientAsync<User, long> _UserClient;
        private IAdminEntityClientAsync<UserRoleMembership, long> _UserRoleMembershipClient;

        public RandomUserGeneratorService(IAdminEntityClientAsync<User, long> userClient, IAdminEntityClientAsync<UserRoleMembership, long> userRoleMembershipClient)
        {
            _UserClient = userClient;
            _UserRoleMembershipClient = userRoleMembershipClient;       
        }

        public async Task<User> CreateRandomUser(int userRoleId)
        {
            var user = new User
            {
                Username = $"TestUser{DateTimeOffset.Now.ToUnixTimeMilliseconds()}{CryptoRandomString.GetCryptoRandomAlphaNumericString(4)}",
                Password = CryptoRandomString.GetCryptoRandomAlphaNumericString(12),
                Enabled = true,
                IsHashed = false
            };
            var odataPostedUser = (await _UserClient.PostAsync(new[] { user })).FirstOrDefault();
            var userRoleMembership = new UserRoleMembership { UserId = odataPostedUser.Id, UserRoleId = userRoleId };
            await _UserRoleMembershipClient.PostAsync(new[] { userRoleMembership });

            user.Id = odataPostedUser != null ? odataPostedUser.Id : 0;
            return user;
        }
    }
}
