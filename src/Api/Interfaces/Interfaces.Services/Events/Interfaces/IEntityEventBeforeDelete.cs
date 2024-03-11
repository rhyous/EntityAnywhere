namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforeDelete<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void BeforeDelete(TEntity entity);
    }
}
