namespace Rhyous.WebFramework.Interfaces
{
    public interface IUser : IId, IAuditable
    {
        string Username { get; set; }
        string OrganizationId { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        bool Active { get; set; }
        bool ExternalAuth { get; set; }
    }
}
