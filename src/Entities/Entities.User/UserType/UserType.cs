using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserType entity. This stores the type of user: System, Internal, Partner, Customer, Organization, Group, etc.
    /// </summary>
    [UserTypeSeedData]
    [AlternateKey("Type")]
    [DisplayNameProperty("Type")]
    public partial class UserType : AuditableEntity<int>, IUserType
    {
        /// <inheritdoc />
        [Required]
        public string Type { get; set; }
    }
}
