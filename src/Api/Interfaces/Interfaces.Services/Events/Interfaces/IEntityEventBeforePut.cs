namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforePut<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void BeforePut(TEntity newEntity, TEntity existingEntity);
    }
}
