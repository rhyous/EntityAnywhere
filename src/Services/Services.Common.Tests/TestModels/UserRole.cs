using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    [RelatedEntityMapping("User", "UserRoleMembership", "UserRole")]
    public class UserRole : Entity<int>, IUserRole
    {
        public string Name { get; set; }
    }
}
