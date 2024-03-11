using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IUserGroupMembership : IBaseEntity<long>, IMappingEntity<long, int>
    {
        long UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
