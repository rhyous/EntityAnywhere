namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterPut<TEntity> : IEntityEvent<TEntity>
    {
        void AfterPut(TEntity newEntity, TEntity priorEntity);
    }
}
