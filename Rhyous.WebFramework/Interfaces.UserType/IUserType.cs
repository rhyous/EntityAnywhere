namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserType : IId<int>, IAuditable
    {
        string Type { get; set; }
    }
}
