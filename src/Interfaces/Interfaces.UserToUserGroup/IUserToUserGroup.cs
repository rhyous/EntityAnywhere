namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserGroup : IEntity<long>
    {
        int UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
