namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPut<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void AfterPut(TEntity newEntity, TEntity priorEntity);
    }
}
