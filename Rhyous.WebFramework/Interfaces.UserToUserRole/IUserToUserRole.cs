namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserToUserRole : IId
    {
        int UserId { get; set; }
        int UserRoleId { get; set; }
    }
}
