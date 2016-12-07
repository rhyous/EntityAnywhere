namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserType : IId, IAuditable
    {
        string Type { get; set; }
    }
}
