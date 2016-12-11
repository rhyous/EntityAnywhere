namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUser : IId<long>, IAuditable, IActivateable
    {
        string Username { get; set; }
        string OrganizationId { get; set; }
        string Password { get; set; }
        string Salt { get; set; }
        bool ExternalAuth { get; set; }
    }
}
