namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserRole : IId<int>, IName, IDescription, IAuditable, IActivateable
    {
    }
}
