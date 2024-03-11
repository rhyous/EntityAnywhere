using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User", AutoExpand = true)]
    public class User : BaseEntity<int>, IUser
    {
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }
        public string Name { get; set; }
    }
}