using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The user entity. This is a user for logging in with an Authenticator plugin.
    /// </summary>
    [AlternateKey("Username")]
    [RelatedEntityMapping("UserGroup", "UserGroupMembership", "User")]
    [RelatedEntityMapping("UserRole", "UserRoleMembership", "User")]
    [RelatedEntityMapping("UserType", "UserTypeMap", "User")]
    public partial class User : AuditableEntity<long>, IUser
    {
        /// <inheritdoc />
        public string Username { get; set; }
        /// <inheritdoc />
        public string Password { get; set; }
        /// <inheritdoc />
        public string Salt { get; set; }
        /// <inheritdoc />
        public bool IsHashed { get; set; }
        /// <inheritdoc />
        public bool Enabled { get; set; }
        /// <inheritdoc />
        public bool ExternalAuth { get; set; }
    }
}
