namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforePut<TEntity> : IEntityEvent<TEntity>
    {
        void BeforePut(TEntity newEntity, TEntity existingEntity);
    }
}
