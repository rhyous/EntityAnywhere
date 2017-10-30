using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [RelatedEntityMapping("User", "UserGroupMembership", "UserGroup")]
    public class UserGroup : Entity<int>, IUserGroup
    {
        public string Name { get; set; }
    }
}
