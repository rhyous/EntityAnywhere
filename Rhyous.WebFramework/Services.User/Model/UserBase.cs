using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public class UserBase : IUserBase
    {
        public int Id { get; set; }
        public string OrganizationId { get; set; }
        public string Username { get; set; }
    }
}
