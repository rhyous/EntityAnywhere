namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserType : IEntity<int>, IAuditable
    {
        string Type { get; set; }
    }
}
