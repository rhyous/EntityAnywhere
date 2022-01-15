namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserRole
    ///  - Entity2: User
    /// </summary>
    public partial interface IUserRoleMembership : IBaseEntity<long>, IMappingEntity<int, long>
    {
        int UserRoleId { get; set; }
        long UserId { get; set; }
    }
}
