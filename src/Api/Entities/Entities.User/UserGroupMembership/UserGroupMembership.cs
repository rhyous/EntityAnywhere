using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserGroup"/>
    ///  - Entity2: <see cref="User"/>
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(long))]
    [MappingEntity(Entity1 = "UserGroup", Entity2 = "User")]
    [EntitySettings(Description = "The user group memberships.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
    public partial class UserGroupMembership : BaseEntity<long>, IUserGroupMembership
    {
        /// <inheritdoc />
        [RelatedEntity("UserGroup")]
        [DistinctProperty("MappingGroup")]
        public int UserGroupId { get; set; }
        /// <inheritdoc />
        /// 
        [RelatedEntity("User")]
        [DistinctProperty("MappingGroup")]
        public long UserId { get; set; }
    }
}
