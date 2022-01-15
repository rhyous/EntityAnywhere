using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPatchMany<TEntity, TId> : IEntityEvent<TEntity>
        where TEntity : IId<TId>
    {
        void AfterPatchMany(IEnumerable<PatchedEntityComparison<TEntity, TId>> patchedEntityComparisons);
    }
}