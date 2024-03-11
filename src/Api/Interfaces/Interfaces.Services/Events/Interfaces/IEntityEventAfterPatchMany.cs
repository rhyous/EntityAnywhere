using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPatchMany<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void AfterPatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons);
    }
}