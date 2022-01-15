namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPatch<TEntity, TId> : IEntityEvent<TEntity>
        where TEntity : IId<TId>
    {
        void AfterPatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison);
    }
}
