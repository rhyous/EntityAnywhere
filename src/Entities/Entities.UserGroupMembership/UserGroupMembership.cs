using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserGroup", Entity2 = "User")]
    public partial class UserGroupMembership : IUserGroupMembership
    {
        public long Id { get; set; }

        public int UserId { get; set; }

        public int UserGroupId { get; set; }
    }
}
