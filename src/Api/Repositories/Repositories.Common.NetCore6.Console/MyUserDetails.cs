using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Repositories
{
    internal class MyUserDetails : IUserDetails
    {
        public long UserId => 3;
        public string Username => "Admin";
        public string UserRole => "Admin";
    }
}
