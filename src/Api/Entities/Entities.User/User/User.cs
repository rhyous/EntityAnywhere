using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The user entity. This is a user for logging in with an Authenticator plugin.
    /// </summary>
    [UserSeedData]
    [AlternateKey("Username")]
    [DisplayNameProperty("Username")]
    [RelatedEntityForeign("UserGroupMembership", "User")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User")]
    [RelatedEntityForeign("UserRoleMembership", "User")]
    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityForeign("UserTypeMap", "User")]
    [RelatedEntityMapping("UserType", "UserTypeMap", "User")]
    [EntitySettings(Description = "The users.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
    public partial class User : AuditableEntity<long>, IUser
    {
        /// <inheritdoc />
        [Required]
        [StringLength(450, MinimumLength = 1)]
        public string Username { get; set; }
        /// <inheritdoc />
        [StringLength(450, MinimumLength = 1)]
        public string Firstname { get; set; }
        /// <inheritdoc />
        [StringLength(450, MinimumLength = 1)]
        public string Lastname { get; set; }
        [IgnoreTrim]
        /// <inheritdoc />
        [StringLength(450)]
        public string Password { get; set; }
        /// <inheritdoc />
        [StringLength(255)]
        public string Salt { get; set; }
        /// <inheritdoc />
        public bool IsHashed { get; set; }
        /// <inheritdoc />
        public bool Enabled { get; set; }
        /// <inheritdoc />
        public bool ExternalAuth { get; set; }
        /// <inheritdoc />
        [RelatedEntity(nameof(Country), AllowedNonExistentValue = 0, AllowedNonExistentValueName = "Prefer not to say.")]
        public int CountryId { get; set; }
    }
}
