using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserRole", Entity2 = "User")]
    public partial class UserRoleMembership : IUserRoleMembership, IMappingEntity<int, int>
    {
        public long Id { get; set; }
        public int UserRoleId { get; set; }

        public int UserId { get; set; }
    }
}
