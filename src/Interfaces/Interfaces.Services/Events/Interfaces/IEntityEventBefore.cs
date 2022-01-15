namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventBefore<TEntity, TId>
                   : IEntityEventBeforePatch<TEntity, TId>,
                     IEntityEventBeforePatchMany<TEntity, TId>,
                     IEntityEventBeforePost<TEntity>,
                     IEntityEventBeforePut<TEntity>,
                     IEntityEventBeforeUpdateProperty<TEntity>,
                     IEntityEventBeforeDelete<TEntity>,
                     IEntityEventBeforeDeleteMany<TEntity>
        where TEntity : IId<TId>
    {
    }
}