namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventPost<TEntity>
        : IEntityEventBeforePost<TEntity>,
          IEntityEventAfterPost<TEntity>
    {
    }
}
