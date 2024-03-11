namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventUpdateProperty<TEntity, TId>
        : IEntityEventBeforeUpdateProperty<TEntity, TId>,
          IEntityEventAfterUpdateProperty<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}
