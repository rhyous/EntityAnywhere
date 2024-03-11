namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The UserRole entity. This should be used to put users in Roles and assign UserRoles some authorization claims.
    /// </summary>
    public partial interface ILandingPage : IBaseEntity<int>, IName, IDescription, IAuditable, IEnabled
    { 
    }
}
