namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserGroup
    ///  - Entity2: User
    /// </summary>
    public partial interface IUserGroupMembership : IBaseEntity<long>, IMappingEntity<int, long>
    {
        int UserGroupId { get; set; }
        long UserId { get; set; }
    }
}
