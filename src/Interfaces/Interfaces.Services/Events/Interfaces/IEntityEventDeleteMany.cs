namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventDeleteMany<TEntity, TId>
                   : IEntityEventBeforeDeleteMany<TEntity>,
                     IEntityEventAfterDeleteMany<TEntity, TId>
        where TEntity : IId<TId>
    { }
}