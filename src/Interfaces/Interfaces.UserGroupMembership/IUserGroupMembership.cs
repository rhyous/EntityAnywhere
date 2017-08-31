namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserGroup
    ///  - Entity2: User
    /// </summary>
    public partial interface IUserGroupMembership : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserGroupId { get; set; }
    }
}
