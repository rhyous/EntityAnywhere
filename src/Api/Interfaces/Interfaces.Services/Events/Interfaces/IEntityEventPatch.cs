namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityEventPatch<TEntity, TId>
                   : IEntityEventBeforePatch<TEntity, TId>,
                     IEntityEventAfterPatch<TEntity, TId>
      where TEntity : IId<TId>
    {
    }
}