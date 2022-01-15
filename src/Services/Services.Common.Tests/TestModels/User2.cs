using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User2")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User2", AutoExpand = true)]
    public class User2 : BaseEntity<int>, IUser2
    {
        [RelatedEntity("UserType", ForeignKeyProperty = "Name")]
        public string UserTypeName { get; set; }
        public string Name { get; set; }
    }
}
