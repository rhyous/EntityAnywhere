using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    public partial class UserToUserGroup : IUserToUserGroup
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int UserGroupId { get; set; }
    }
}
