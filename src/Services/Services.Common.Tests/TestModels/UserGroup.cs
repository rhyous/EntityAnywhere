using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [RelatedEntityForeign("UserGroupMembership", "UserGroup")]
    public class UserGroup : Entity<int>, IUserGroup
    {
        public string Name { get; set; }
    }
}
