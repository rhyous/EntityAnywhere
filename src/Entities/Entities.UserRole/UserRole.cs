using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserRole entity. This should be used to put users in Roles and assign UserRoles some authorization claims.
    /// </summary>
    [AlternateKey("Name")]
    public partial class UserRole : AuditableEntity<int>, IUserRole
    {
        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string Description { get; set; }

        /// <inheritdoc />
        public bool Enabled { get; set; }        
    }
}
