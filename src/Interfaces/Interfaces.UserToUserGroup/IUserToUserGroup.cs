namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserToUserGroup : IEntity<long>
    {
        int UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
