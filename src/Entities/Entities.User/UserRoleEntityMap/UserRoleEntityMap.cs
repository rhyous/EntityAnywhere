using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserGroupMembership mapping entity.
    /// Mapped entities:
    ///  - Entity1: <see cref="UserRole"/>
    ///  - Entity2: <see cref="Entity"/>
    /// </summary>
    [EntitySettings(Group = "User Management")]
    [AdditionalWebServiceTypes(typeof(int), typeof(string))]
    [MappingEntity(Entity1 = "UserRole", Entity2 = "Entity")]
    public partial class UserRoleEntityMap : BaseEntity<long>, IUserRoleEntityMap
    {
        /// <inheritdoc />
        [RelatedEntity("UserRole")]
        [DistinctProperty("MappingGroup")]
        public int UserRoleId { get; set; }

        /// <inheritdoc />
        [RelatedEntity("Entity", AllowedNonExistentValue = 0, AllowedNonExistentValueName = "All")]
        [DistinctProperty("MappingGroup")]
        public int EntityId { get; set; }
    }
}