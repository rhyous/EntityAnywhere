namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventDeleteMany<TEntity, TId>
                   : IEntityEventBeforeDeleteMany<TEntity, TId>,
                     IEntityEventAfterDeleteMany<TEntity, TId>
        where TEntity : IId<TId>
    { }
}