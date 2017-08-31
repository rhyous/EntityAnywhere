using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// The UserType entity. This stores the type of user: System, Internal, Partner, Customer, Organization, Group, etc.
    /// </summary>
    [AlternateKey("Type")]
    public partial class UserType : AuditableEntityBase<int>, IUserType
    {
        /// <inheritdoc />
        public string Type { get; set; }
    }
}
