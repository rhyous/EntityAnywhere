using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User", AutoExpand = true)]
    public class User : Entity<int>, IUser
    {
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
        public string Name { get; set; }
    }
}
