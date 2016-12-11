namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserType : IId<long>
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
