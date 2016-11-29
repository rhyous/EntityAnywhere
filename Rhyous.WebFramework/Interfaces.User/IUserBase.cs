namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserBase : IId
    {
        string Username { get; set; }
        string OrganizationId { get; set; }
    }
}
