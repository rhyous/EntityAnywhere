namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The UserTypeMap mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserType
    ///  - Entity2: User
    /// </summary>
    public partial interface IUserTypeMap : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
