namespace Rhyous.EntityAnywhere.Interfaces
{
    public class PatchedEntityComparison<T, TId>
        where T : IId<TId>
    {
        /// <summary>
        /// The PatchedEntity.
        /// </summary>
        /// <remarks>The entity may only have the changed properties set.</remarks>
        public PatchedEntity<T, TId> PatchedEntity { get; set; }

        /// <summary>
        /// The entity before the changes
        /// </summary>
        public T Entity { get; set; }
    }
}