using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [RelatedEntityForeign("UserRoleMembership", "UserRole")]
    public class UserRole : Entity<int>, IUserRole
    {
        public string Name { get; set; }
    }
}
