using Rhyous.Odata;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// This entity represents a Group into which a <see cref="Organization"/> can be added.
    /// </summary>
    [AlternateKey("Name")]
    [RelatedEntityForeign("OrganizationGroupEarlyRelease", "OrganizationGroup")]
    [RelatedEntityForeign("OrganizationGroupMembership", "OrganizationGroup", ForeignKeyProperty = "OrganizationGroupId")]
    [RelatedEntityMapping("Organization", "OrganizationGroupMembership", "OrganizationGroup")]
    public class OrganizationGroup : AuditableEntity<int>, IOrganizationGroup
    {
        /// <inheritdoc />
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
