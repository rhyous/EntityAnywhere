using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityForeign("UserGroupMembership", "UserGroup")]
    public class UserGroup : BaseEntity<int>, IUserGroup
    {
        public string Name { get; set; }
    }
}
