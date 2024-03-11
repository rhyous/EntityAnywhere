namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfterUpdateProperty<TEntity, TId> : IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    {
        void AfterUpdateProperty(string property, object newValue, object existingValue);
    }
}