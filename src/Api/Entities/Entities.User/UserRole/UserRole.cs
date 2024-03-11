using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserRole entity. This should be used to put users in Roles and assign UserRoles some authorization claims.
    /// </summary>
    [UserRoleSeedData]
    [AlternateKey(nameof(Name))]
    [RelatedEntityForeign(nameof(UserRoleEntityMap), nameof(UserRole))]
    [RelatedEntityMapping("Entity", nameof(UserRoleEntityMap), nameof(UserRole))]
    [RelatedEntityForeign(nameof(UserRoleMembership), nameof(UserRole))]
    [RelatedEntityMapping(nameof(User), nameof(UserRoleMembership), nameof(UserRole))]
    [EntitySettings(Description = "The user roles.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
    public partial class UserRole : AuditableEntity<int>, IUserRole
    {
        /// <inheritdoc />
        [Required]
        public string Name { get; set; }
        /// <inheritdoc />
        [IgnoreTrim]
        public string Description { get; set; }

        /// <inheritdoc />
        public bool Enabled { get; set; }

        [RelatedEntity(nameof(LandingPage))]
        public int LandingPageId { get; set; }
    }
}