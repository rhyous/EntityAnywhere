namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IUserTypeMap : IEntity<long>, IMappingEntity<int, int>
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
