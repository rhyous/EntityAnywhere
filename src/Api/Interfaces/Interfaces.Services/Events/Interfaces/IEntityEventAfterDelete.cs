namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterDelete<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void AfterDelete(TEntity entity, bool wasDeleted);
    }
}