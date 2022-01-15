namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventPut<TEntity>
        : IEntityEventBeforePut<TEntity>,
          IEntityEventAfterPut<TEntity>
    {
    }
}
