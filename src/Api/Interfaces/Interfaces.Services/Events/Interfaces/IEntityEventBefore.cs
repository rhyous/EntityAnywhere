namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBefore<TEntity, TId>
                   : IEntityEventBeforePatch<TEntity, TId>,
                     IEntityEventBeforePatchMany<TEntity, TId>,
                     IEntityEventBeforePost<TEntity, TId>,
                     IEntityEventBeforePut<TEntity, TId>,
                     IEntityEventBeforeUpdateProperty<TEntity, TId>,
                     IEntityEventBeforeDelete<TEntity, TId>,
                     IEntityEventBeforeDeleteMany<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}