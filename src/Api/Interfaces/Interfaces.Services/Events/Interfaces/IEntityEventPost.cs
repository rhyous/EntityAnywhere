namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventPost<TEntity, TId>
        : IEntityEventBeforePost<TEntity, TId>,
          IEntityEventAfterPost<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}
