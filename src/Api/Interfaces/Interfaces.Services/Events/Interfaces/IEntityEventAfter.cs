namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventAfter<TEntity, TId>
                   : IEntityEventAfterPatch<TEntity, TId>,
                     IEntityEventAfterPatchMany<TEntity, TId>,
                     IEntityEventAfterPost<TEntity, TId>,
                     IEntityEventAfterPut<TEntity, TId>,
                     IEntityEventAfterUpdateProperty<TEntity, TId>,
                     IEntityEventAfterDelete<TEntity, TId>,
                     IEntityEventAfterDeleteMany<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}