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
    [UserRoleEntityMapSeedData]
    [EntitySettings(Description = "A mapping between User Roles and Entities, used to authorize entities.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
    [AdditionalWebServiceTypes(typeof(int), typeof(int))]
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