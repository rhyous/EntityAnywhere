namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventDelete<TEntity> 
        : IEntityEventBeforeDelete<TEntity>,
          IEntityEventAfterDelete<TEntity>
    { }
}