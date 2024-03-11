using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public class ClaimsBuilderAsync : IClaimsBuilderAsync
    {
        private readonly IAdminEntityClientAsync<ClaimConfiguration, int> ClaimConfigurationClient;
        private readonly IAdminEntityClientAsync<User, long> UserClient;

        public ClaimsBuilderAsync(
            IAdminEntityClientAsync<ClaimConfiguration, int> claimConfigurationClient,
            IAdminEntityClientAsync<User, long> userClient
        ) {
            ClaimConfigurationClient = claimConfigurationClient;
            UserClient = userClient;
        }

        public async Task<IUser> BuildAsync(long userId, IToken token)
        {
            var claimConfigs = await ClaimConfigurationClient.GetAllAsync();
            var relatedEntitiesToExpand = claimConfigs?.Where(c => c.Object.Entity != "User").Select(c => c.Object.Entity).Distinct();

            var odataUser = await UserClient.GetAsync(userId.ToString(), $"$expand={string.Join(",", relatedEntitiesToExpand)}") ?? throw new Exception("User not found.");
            var user = odataUser.Object;

            var claims = await BuildAsync(user, claimConfigs?.Select(c => c.Object), odataUser.RelatedEntityCollection);
            if (claims != null && claims.Count > 0)
                token.ClaimDomains.AddRange(claims);
            Task.WaitAll();
            return user;
        }

        #pragma warning disable 1998
        public async Task<List<ClaimDomain>> BuildAsync(IUser user, IEnumerable<IClaimConfiguration> claimsConfigurations, IList<RelatedEntityCollection> relatedEntityCollections)
        {
            if (user == null || user.Id <= 0 || claimsConfigurations == null || !claimsConfigurations.Any())
                return null;
            var claimDomains = new ClaimDomainDictionary();
            BuildClaimsFromUserEntity(claimDomains, user, claimsConfigurations);
            BuildClaimsFromRelatedEntities(claimDomains, claimsConfigurations, relatedEntityCollections);
            return claimDomains.Values.ToList();
        }
        
        internal void BuildClaimsFromUserEntity(ClaimDomainDictionary claimDomains, IUser user, IEnumerable<IClaimConfiguration> claimsConfigurations)
        {
            if (claimDomains == null || user == null)
                return;
            if (claimsConfigurations != null && claimsConfigurations.Any())
            {
                var userClaimConfigs = claimsConfigurations.Where(cc => cc.Entity == "User");
                foreach (var claimConfig in userClaimConfigs)
                {
                    var domainKey = string.IsNullOrWhiteSpace(claimConfig.Domain) ? claimConfig.Entity : claimConfig.Domain;
                    var claimDomain = claimDomains[domainKey];
                    claimDomain.Claims.Add(BuildUserClaim(user, claimConfig));
                }
            }
            var now = DateTimeOffset.Now.ToUniversalTime();
            var lastAuthenticated = new Claim { Name = "LastAuthenticated", Value = now.ToString(DateTimeFormatInfo.CurrentInfo.RFC1123Pattern) };
            claimDomains["User"].Claims.Add(lastAuthenticated);
        }
        
        internal Claim BuildUserClaim(IUser user, IClaimConfiguration claimConfig)
        {
            var value = user.GetPropertyValue(claimConfig.EntityProperty).ToString();
            return new Claim { Name = claimConfig.Name, Value = value };
        }
        
        internal void BuildClaimsFromRelatedEntities(ClaimDomainDictionary claimDomains, IEnumerable<IClaimConfiguration> claimsConfigurations, IList<RelatedEntityCollection> relatedEntityCollections)
        {
            var relatedEntityClaimConfigs = claimsConfigurations.Where(cc => cc.Entity != "User");
            foreach (var claimConfig in relatedEntityClaimConfigs)
            {
                var domainKey = string.IsNullOrWhiteSpace(claimConfig.Domain) ? claimConfig.Entity : claimConfig.Domain;
                var domain = claimDomains[domainKey];
                var relatedEntityCollection = relatedEntityCollections.FirstOrDefault(re => re.RelatedEntity == claimConfig.Entity);
                if (relatedEntityCollection == null || !relatedEntityCollection.Any())
                    continue;
                var claims = BuildRelatedEntityClaim(relatedEntityCollection, claimConfig);
                domain.Claims.AddRange(claims);
            }
        }

        internal List<Claim> BuildRelatedEntityClaim(RelatedEntityCollection relatedEntityCollection, IClaimConfiguration claimConfig)
        {
            var values = JsonClaimsParser.Parse(claimConfig, relatedEntityCollection);
            var list = new List<Claim>();
            foreach (var value in values)
            {
                list.Add(new Claim { Name = claimConfig.Name, Value = value });
            }
            return list;
        }
    }
}