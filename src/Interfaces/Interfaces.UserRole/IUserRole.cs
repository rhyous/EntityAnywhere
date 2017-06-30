namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserRole : IEntity<int>, IName, IDescription, IAuditable, IEnabled
    {
    }
}
