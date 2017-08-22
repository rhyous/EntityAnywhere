namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserRoleMembership : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserRoleId { get; set; }
    }
}
