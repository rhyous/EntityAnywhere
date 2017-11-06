using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserGroup"/>
    ///  - Entity2: <see cref="User"/>
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserGroup", Entity2 = "User")]
    public partial class UserGroupMembership : Entity<long>, IUserGroupMembership
    {
        /// <inheritdoc />
        [RelatedEntity("UserGroup")]
        public int UserGroupId { get; set; }
        /// <inheritdoc />
        [RelatedEntity("User")]
        public int UserId { get; set; }
    }
}
