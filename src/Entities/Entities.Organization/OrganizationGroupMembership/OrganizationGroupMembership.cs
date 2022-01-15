using Rhyous.Odata;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The is a mapping entity that maps a <see cref="Organization"/> to a <see cref="OrganizationGroup"/>.
    /// </summary>
    public class OrganizationGroupMembership : AuditableEntity<int>, IOrganizationGroupMembership
    {
        /// <inheritdoc />
        [RelatedEntity("OrganizationGroup")]
        [DistinctProperty("MappingGroup")]
        public int OrganizationGroupId { get; set; }

        /// <inheritdoc />
        [RelatedEntity("Organization")]
        [DistinctProperty("MappingGroup")]
        public int OrganizationId { get; set; }
    }
}
