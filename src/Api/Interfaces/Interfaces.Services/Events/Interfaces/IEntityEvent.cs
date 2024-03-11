namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEvent<TEntity, TId>
        where TEntity : IId<TId>
    { }
}
