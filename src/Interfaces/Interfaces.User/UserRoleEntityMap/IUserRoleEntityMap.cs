namespace Rhyous.EntityAnywhere.Interfaces
{
    public partial interface IUserRoleEntityMap : IBaseEntity<long>, IMappingEntity<int, string>
    {
        int UserRoleId { get; set; }
        int EntityId { get; set; }
    }
}