namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterDelete<TEntity> : IEntityEvent<TEntity>
    {
        void AfterDelete(TEntity entity, bool wasDeleted);
    }
}