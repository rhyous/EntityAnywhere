namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventDelete<TEntity, TId> 
        : IEntityEventBeforeDelete<TEntity, TId>,
          IEntityEventAfterDelete<TEntity, TId>
        where TEntity : IId<TId>
    { }
}