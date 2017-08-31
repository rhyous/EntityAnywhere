using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserGroup
    ///  - Entity2: User
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserGroup", Entity2 = "User")]
    public partial class UserGroupMembership : EntityBase<long>, IUserGroupMembership
    {
        /// <inheritdoc />
        public int UserId { get; set; }
        /// <inheritdoc />
        public int UserGroupId { get; set; }
    }
}
