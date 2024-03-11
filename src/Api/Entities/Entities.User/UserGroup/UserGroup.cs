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
    [EntitySettings(Description = "The user groups.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
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
