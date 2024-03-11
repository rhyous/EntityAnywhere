namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPatch<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void AfterPatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison);
    }
}
