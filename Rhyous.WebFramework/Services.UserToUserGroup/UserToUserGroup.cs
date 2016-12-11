using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class UserToUserGroup : IUserToUserGroup
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int UserGroupId { get; set; }
    }
}
