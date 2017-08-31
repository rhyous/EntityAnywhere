namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserRole
    ///  - Entity2: User
    /// </summary>
    public partial interface IUserRoleMembership : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserRoleId { get; set; }
    }
}
