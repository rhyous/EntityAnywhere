namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IUserDetails
    {
        long UserId { get; }
        string Username { get; }
        string UserRole { get; }
        int OrganizationId { get; }
    }
}