namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserRole : IEntity<long>
    {
        int UserId { get; set; }
        int UserRoleId { get; set; }
    }
}
