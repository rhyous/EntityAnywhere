using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforePatchMany<TEntity, TId> : IEntityEvent<TEntity>
        where TEntity : IId<TId>
    {
        void BeforePatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons);
    }
}