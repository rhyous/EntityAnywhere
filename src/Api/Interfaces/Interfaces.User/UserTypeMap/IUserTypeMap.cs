namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The UserTypeMap mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserType
    ///  - Entity2: User
    /// </summary>
    public partial interface IUserTypeMap : IBaseEntity<long>, IMappingEntity<int, long>
    {
        int UserTypeId { get; set; }
        long UserId { get; set; }
    }
}
