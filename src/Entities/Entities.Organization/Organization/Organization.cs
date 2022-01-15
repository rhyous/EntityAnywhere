using Rhyous.Odata;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// This represents and organization. An organization can be either Internal, Distributor, Partner, or Customer.
    /// </summary>
    [AlternateKey("SapId")]
    [OrganizationSeedData]
    [RelatedEntityForeign("User", nameof(Organization))]
    [RelatedEntityForeign("Entitlement", nameof(Organization))]
    [RelatedEntityForeign("ActivationCredential", nameof(Organization))]
    [RelatedEntityForeign("DecentralizedIdentity", nameof(Organization))]
    [RelatedEntityForeign("EntitledProductUsage", nameof(Organization))]
    [RelatedEntityForeign("OrganizationGroupMembership", nameof(Organization))]
    [RelatedEntityMapping("OrganizationGroup", "OrganizationGroupMembership", nameof(Organization))]
    [RelatedEntityForeign("ProductReleaseOverride", nameof(Organization))]
    [RelatedEntityForeign("Tenant", nameof(Organization))]
    public class Organization : AuditableEntity<int>, IOrganization
    {
        /// <inheritdoc />
        [Required]
        public virtual string Name { get; set; }

        /// <inheritdoc />
        public virtual string Description { get; set; }

        public string SapId { get; set; }

        /// <inheritdoc />
        public OrganizationCategory Category { get; set; }
    }
}