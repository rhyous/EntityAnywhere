using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforePatchMany<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void BeforePatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons);
    }
}