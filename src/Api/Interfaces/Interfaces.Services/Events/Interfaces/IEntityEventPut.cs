namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventPut<TEntity, TId>
        : IEntityEventBeforePut<TEntity, TId>,
          IEntityEventAfterPut<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}
