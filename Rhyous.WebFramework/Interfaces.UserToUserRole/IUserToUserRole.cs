namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserRole : IId<long>
    {
        int UserId { get; set; }
        int UserRoleId { get; set; }
    }
}
