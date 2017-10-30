using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    public interface IUserGroupMembership : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
