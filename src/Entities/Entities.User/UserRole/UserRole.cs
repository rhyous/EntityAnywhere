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
    [AlternateKey("Name")]
    [RelatedEntityForeign("UserRoleEntityMap", nameof(UserRole))]
    [RelatedEntityMapping("Entity", "UserRoleEntityMap", nameof(UserRole))]
    [RelatedEntityForeign("UserRoleMembership", nameof(UserRole))]
    [RelatedEntityMapping("User", "UserRoleMembership", nameof(UserRole))]
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