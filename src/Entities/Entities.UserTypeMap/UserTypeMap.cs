using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// The UserTypeMap mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserType"/>
    ///  - Entity2: <see cref="User"/>
    /// </summary>
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
    [MappingEntity(Entity1 = "UserType", Entity2 = "User")]
    public partial class UserTypeMap : Entity<long>, IUserTypeMap, IMappingEntity<int, int>
    {
        /// <inheritdoc />
        [RelatedEntity("UserType")]
        public int UserTypeId { get; set; }

        /// <inheritdoc />
        [RelatedEntity("User")]
        public int UserId { get; set; }
    }
}
