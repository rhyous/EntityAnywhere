namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforePatch<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void BeforePatch(PatchedEntityComparison<TEntity, TId> patchedEntityComparison);
    }
}