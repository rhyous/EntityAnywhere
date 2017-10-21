using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    public interface IMappingCachedEntityClient<TEntity, TId, TE1Id, TE2Id> : IEntityCache<TEntity, TId>
    {
        Dictionary<TId, TE1Id> CacheByE1Id { get; set; }
        Dictionary<TId, TE2Id> CacheByE2Id { get; set; }
    }
}
