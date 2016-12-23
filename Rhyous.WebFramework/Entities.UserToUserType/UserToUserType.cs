using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class UserToUserType : IUserToUserType
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
    }
}
