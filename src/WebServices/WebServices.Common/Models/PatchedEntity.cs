using System.Collections.Generic;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object is used to update more than one property on an entity without first getting the entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class PatchedEntity<TEntity>
    {
        /// <summary>
        /// A stub of the entity. Only the changed properties need to be populated.
        /// </summary>
        public TEntity Entity { get; set; }

        /// <summary>
        /// A list of the changed property names.
        /// </summary>
        public List<string> ChangedProperties { get; set; }
    }
}
