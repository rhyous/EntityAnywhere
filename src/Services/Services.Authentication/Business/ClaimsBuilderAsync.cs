using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public class ClaimsBuilderAsync : IClaimsBuilderAsync
    {
        public ClaimsBuilderAsync() { }
        public ClaimsBuilderAsync(IEntityClientCache clientsCache) { ClientsCache = clientsCache; }

        public async Task<List<ClaimDomain>> BuildAsync(IUser user, IEnumerable<ClaimConfiguration> claimsConfigurations)
        {
            if (user == null || user.Id <= 0 || claimsConfigurations == null || !claimsConfigurations.Any())
                return null;
            var claimDomains = new ClaimDomainDictionary();
            foreach (var claimConfig in claimsConfigurations)
            {
                await BuildClaimAsync(user, claimDomains, claimConfig);
            }
            return claimDomains.Values.ToList();
        }

        internal async Task BuildClaimAsync(IUser user, ClaimDomainDictionary claimDomains, ClaimConfiguration claimConfig)
        {
            var domainName = string.IsNullOrWhiteSpace(claimConfig.Domain) ? claimConfig.Entity : claimConfig.Domain;
            var domain = claimDomains[domainName];
            if (claimConfig.Entity == "User")
            {
                var value = user.GetPropertyValue(claimConfig.EntityProperty).ToString();
                domain.Claims.Add(new Claim { Name = claimConfig.Name, Value = value });
            }
            else
            {
                domain.Claims.AddRange((await BuildClaimFromEntityAsync(user, claimConfig)));
            }
        }

        internal async Task<List<Claim>> BuildClaimFromEntityAsync(IUser user, ClaimConfiguration claimConfig)
        {
            var entities = await GetEntities(claimConfig.Entity, claimConfig.EntityIdProperty, user.GetPropertyValue(claimConfig.RelatedEntityIdProperty, "Id").ToString());
            if (entities == null || !entities.Any())
                return null;
            var claims = new List<Claim>();
            foreach (var entity in entities)
            {
                var jobject = JObject.Parse(entity.Object.ToString());
                var value = jobject[claimConfig.EntityProperty]?.ToObject<string>();
                claims.Add(new Claim { Name = claimConfig.Name, Value = value });
            }
            return claims;
        }

        internal async Task<List<RelatedEntity>> GetEntities(string entity, string relatedIdProperty, string id)
        {
            var client = ClientsCache.Json[entity];
            var entitiesJson = await client.GetAllAsync($"?$filter={relatedIdProperty} eq {id}");
            if (string.IsNullOrWhiteSpace(entitiesJson))
                return null;
            var entities = JsonConvert.DeserializeObject<List<RelatedEntity>>(entitiesJson);
            return entities;
        }

        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;
    }
}