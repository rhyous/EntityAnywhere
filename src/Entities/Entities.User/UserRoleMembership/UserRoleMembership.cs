using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserRole"/>
    ///  - Entity2: <see cref="User"/>
    /// </summary>
    [UserRoleMembershipSeedData]
    [AdditionalWebServiceTypes(typeof(int), typeof(long))]
    [MappingEntity(Entity1 = "UserRole", Entity2 = "User")]
    public partial class UserRoleMembership : BaseEntity<long>, IUserRoleMembership
    {
        /// <inheritdoc />
        [RelatedEntity("UserRole")]
        [DistinctProperty("MappingGroup")]
        public int UserRoleId { get; set; }
        /// <inheritdoc />
        [RelatedEntity("User")]
        [DistinctProperty("MappingGroup")]
        public long UserId { get; set; }
    }
}