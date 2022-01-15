using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityForeign("UserRoleMembership", "UserRole")]
    public class UserRole : BaseEntity<int>, IUserRole
    {
        public string Name { get; set; }
    }
}
