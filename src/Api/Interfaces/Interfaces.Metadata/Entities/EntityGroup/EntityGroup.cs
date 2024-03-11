using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The group name for an Entity.
    /// </summary>
    [AlternateKey(nameof(Name))]
    [RelatedEntityExclusions("*")]
    [EntitySettings(Description = "A list of Entity Groups.",
                    Group = "Entity Settings",
                    GroupDescription = "A group for configuring Entity Settings.")]
    public class EntityGroup : AuditableEntity<int>, IEntityGroup
    {
        /// <summary>
        /// The name of an entity group.
        /// This is an alternate key.
        /// This should be unique.
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
