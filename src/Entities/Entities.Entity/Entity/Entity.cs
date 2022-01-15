using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// An Entity about Entities.
    /// </summary>
    [AlternateKey(nameof(Name))]
    [RelatedEntityForeign("EntityProperty", nameof(Entity))]
    [RelatedEntityForeign("UserRoleEntityMap", nameof(Entity))]
    [RelatedEntityMapping("UserRole", "UserRoleEntityMap", nameof(Entity))]
    public class Entity : AuditableEntity<int>, IEntity
    {
        /// <summary>
        /// The name of the Entity.
        /// This is an alternate key.
        /// This should be unique.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the Entity
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Whether the entity is enabled.
        /// </summary>
        /// <remarks>This does nothing yet, but we should disable the Entity and the Entity Web Service endpoint.</remarks>
        public bool Enabled { get; set; }

        /// <summary>
        /// An Entity Group. This will allow for Entities to be grouped in the UI.
        /// </summary>
        [RelatedEntity("EntityGroup")]
        public int EntityGroupId { get; set; }

        /// <summary>
        /// The primary key reference to an EntityProperty.Id
        /// </summary>
        [RelatedEntity("EntityProperty", RelatedEntityAlias = "SortByProperty", AllowedNonExistentValue = 0)]
        public int? SortByPropertyId { get; set; } = 0;

        /// <summary>
        /// Whether to sort by Ascending or Descending.
        /// </summary>
        public SortOrder SortOrder { get; set; }
    }
}
