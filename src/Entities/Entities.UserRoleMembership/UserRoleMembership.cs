using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserRole"/>
    ///  - Entity2: <see cref="User"/>
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserRole", Entity2 = "User")]
    public partial class UserRoleMembership : Entity<long>, IUserRoleMembership, IMappingEntity<int, int>
    {
        /// <inheritdoc />
        [RelatedEntity("UserRole")]
        public int UserRoleId { get; set; }
        /// <inheritdoc />
        [RelatedEntity("User")]
        public int UserId { get; set; }
    }
}
