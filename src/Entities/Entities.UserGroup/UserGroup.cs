using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    [AlternateKey("Name")]
    public partial class UserGroup : AuditableEntityBase<int>, IUserGroup
    {
        /// <inheritdoc />
        public string Name { get; set; }
        /// <inheritdoc />
        public string Description { get; set; }        
    }
}
