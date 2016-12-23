using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    public partial class UserToUserRole : IUserToUserRole
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
    }
}
