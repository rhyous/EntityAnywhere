using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserType", Entity2 = "User")]
    public partial class UserTypeMap : IUserTypeMap, IMappingEntity<int, int>
    {
        public long Id { get; set; }
        public int UserTypeId { get; set; }

        public int UserId { get; set; }
    }
}
