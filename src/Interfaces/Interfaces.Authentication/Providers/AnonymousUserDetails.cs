namespace Rhyous.EntityAnywhere.Interfaces
{
    public class AnonymousUserDetails : IUserDetails
    {
        public long UserId => 0;
        public string Username => "Anonymous";
        public string UserRole => "Anonymous";
        public int OrganizationId => 0;
    }
}