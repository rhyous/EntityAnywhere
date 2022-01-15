namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterUpdateProperty<TEntity> : IEntityEvent<TEntity>
    {
        void AfterUpdateProperty(string property, object newValue, object existingValue);
    }
}