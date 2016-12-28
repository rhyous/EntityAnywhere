namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserType : IEntity<long>
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
