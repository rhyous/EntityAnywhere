namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class PatchedEntityExtensions
    {
        public static PatchedEntity<TInterface, TId> AsInterface<TEntity, TInterface, TId>(this PatchedEntity<TEntity, TId> patchedEntity)
            where TEntity : class, TInterface
            where TInterface : IId<TId>
        {
            return new PatchedEntity<TInterface, TId>
            {
                Entity = patchedEntity?.Entity,
                ChangedProperties = patchedEntity?.ChangedProperties
            };
        }

        public static PatchedEntity<TEntity, TId> ToConcrete<TEntity, TInterface, TId>(this PatchedEntity<TInterface, TId> patchedEntity)
            where TEntity : class, TInterface
            where TInterface : IId<TId>
        {
            return new PatchedEntity<TEntity, TId>
            {
                Entity = patchedEntity as TEntity,
                ChangedProperties = patchedEntity?.ChangedProperties
            };
        }
    }
}