namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfter<TEntity, TId>
                   : IEntityEventAfterPatch<TEntity, TId>,
                     IEntityEventAfterPatchMany<TEntity, TId>,
                     IEntityEventAfterPost<TEntity>,
                     IEntityEventAfterPut<TEntity>,
                     IEntityEventAfterUpdateProperty<TEntity>,
                     IEntityEventAfterDelete<TEntity>,
                     IEntityEventAfterDeleteMany<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}