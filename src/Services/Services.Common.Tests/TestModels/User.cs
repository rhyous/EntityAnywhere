using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [RelatedEntityForeign("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityForeign("UserGroup", "UserGroupMembership", "User", AutoExpand = true)]
    public class User : Entity<int>, IUser
    {
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
        public string Name { get; set; }
    }
}
