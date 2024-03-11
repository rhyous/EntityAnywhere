using Rhyous.Collections;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Cache
{
    internal class EntityGroupCache : CacheBase<IEntityGroupDictionary>, IEntityGroupCache
    {
        private const int MaxEntityGroups = 100;
        private readonly IAdminEntityClientAsync<EntityGroup, int> _EntityGroupClient;

        public EntityGroupCache(IAdminEntityClientAsync<EntityGroup, int> entityGroupClient)
            : base(new EntityGroupDictionary())
        {
            _EntityGroupClient = entityGroupClient;
        }

        protected override async Task CreateCacheAsync()
        {
            var odataGroups = await _EntityGroupClient.GetByQueryParametersAsync($"$top={MaxEntityGroups}");
            if (odataGroups != null && odataGroups.Any())
            {
                foreach (var g in odataGroups)
                {
                    _Cache.TryAdd(g.Object.Name, g.Object);
                }
            }
        }
    }
}