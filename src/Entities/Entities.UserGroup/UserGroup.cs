using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserGroup entity.
    /// </summary>
    [AlternateKey("Name")]
    public partial class UserGroup : AuditableEntity<int>, IUserGroup
    {
        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string Description { get; set; }        
    }
}
