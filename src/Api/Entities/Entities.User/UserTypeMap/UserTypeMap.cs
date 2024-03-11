using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserTypeMap mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserType"/>
    ///  - Entity2: <see cref="User"/>
    /// </summary>
    [UserTypeMapMapSeedData]
    [AdditionalWebServiceTypes(typeof(int), typeof(long))]
    [MappingEntity(Entity1 = "UserType", Entity2 = "User")]
    [EntitySettings(Description = "The user type maps.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
    public partial class UserTypeMap : BaseEntity<long>, IUserTypeMap
    {
        /// <inheritdoc />
        [RelatedEntity("UserType")]
        [DistinctProperty("MappingGroup")]
        public int UserTypeId { get; set; }

        /// <inheritdoc />
        [RelatedEntity("User")]
        [DistinctProperty("MappingGroup")]
        public long UserId { get; set; }
    }
}
