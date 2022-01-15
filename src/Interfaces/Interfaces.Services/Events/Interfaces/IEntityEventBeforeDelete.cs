namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforeDelete<TEntity> : IEntityEvent<TEntity>
    {
        void BeforeDelete(TEntity entity);
    }
}
