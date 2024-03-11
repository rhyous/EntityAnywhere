namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAll<TEntity, TId>
                   : IEntityEventBefore<TEntity, TId>,
                     IEntityEventAfter<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}