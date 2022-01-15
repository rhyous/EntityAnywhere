namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventPatchMany<TEntity, TId>
                   : IEntityEventBeforePatchMany<TEntity, TId>,
                     IEntityEventAfterPatchMany<TEntity, TId>
        where TEntity : IId<TId>
    {
    }
}