namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The UserRole entity. This should be used to put users in Roles and assign UserRoles some authorization claims.
    /// </summary>
    public partial interface IUserRole : IEntity<int>, IName, IDescription, IAuditable, IEnabled
    {
    }
}
