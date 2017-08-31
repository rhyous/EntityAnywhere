using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    [AlternateKey("Username")]
    public partial class User : AuditableEntityBase<long>, IUser
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
