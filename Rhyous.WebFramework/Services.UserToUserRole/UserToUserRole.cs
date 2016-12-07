using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class UserToUserRole : IUserToUserRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
    }
}
