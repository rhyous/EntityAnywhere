namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforeUpdateProperty<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void BeforeUpdateProperty(string property, object newValue, object existingValue);
    }
}
