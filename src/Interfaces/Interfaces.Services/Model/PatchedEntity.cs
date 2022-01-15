using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This object is used to update more than one property on an entity without first getting the entity.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public class PatchedEntity<T, TId>
        where T : IId<TId>
    {
        /// <summary>
        /// A stub of the entity. Only the changed properties need to be populated.
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// A list of property names changed only on this entity.
        /// </summary>
        public HashSet<string> ChangedProperties { get; set; }
    }
}
