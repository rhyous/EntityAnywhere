namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserRole : IId
    {
        int UserId { get; set; }
        int UserRoleId { get; set; }
    }
}
