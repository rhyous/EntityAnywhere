using Rhyous.WebFramework.Interfaces.Attributes;

namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserGroupMembership : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
