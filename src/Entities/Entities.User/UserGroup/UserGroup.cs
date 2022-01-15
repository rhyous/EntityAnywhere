using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserGroup entity.
    /// </summary>
    [AlternateKey("Name")]
    public partial class UserGroup : AuditableEntity<int>, IUserGroup
    {
        /// <inheritdoc />
        [Required]
        public string Name { get; set; }
        /// <inheritdoc />
        [IgnoreTrim]
        public string Description { get; set; }        
    }
}
