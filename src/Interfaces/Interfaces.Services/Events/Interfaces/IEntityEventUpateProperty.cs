namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventUpdateProperty<TEntity>
        : IEntityEventBeforeUpdateProperty<TEntity>,
          IEntityEventAfterUpdateProperty<TEntity>
    {
    }
}
