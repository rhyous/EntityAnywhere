using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserRole
    ///  - Entity2: User
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserRole", Entity2 = "User")]
    public partial class UserRoleMembership : Entity<long>, IUserRoleMembership, IMappingEntity<int, int>
    {
        /// <inheritdoc />
        public int UserRoleId { get; set; }
        /// <inheritdoc />
        public int UserId { get; set; }
    }
}
