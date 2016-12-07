namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserToUserType : IId
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
