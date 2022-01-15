namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBeforeUpdateProperty<TEntity> : IEntityEvent<TEntity>
    {
        void BeforeUpdateProperty(string property, object newValue, object existingValue);
    }
}
