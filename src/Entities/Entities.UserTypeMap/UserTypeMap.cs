using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// The UserTypeMap mapping entity.
    /// Mapped entities:
    ///  - Entity1: UserType
    ///  - Entity2: User
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserType", Entity2 = "User")]
    public partial class UserTypeMap : EntityBase<long>, IUserTypeMap, IMappingEntity<int, int>
    {
        public int UserTypeId { get; set; }

        public int UserId { get; set; }
    }
}
