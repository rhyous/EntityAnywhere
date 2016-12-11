namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserToUserGroup : IId<long>
    {
        int UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
