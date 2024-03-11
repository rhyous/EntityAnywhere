using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// An entity to store AuthenticationAttempt.
    /// </summary>
    [DisplayNameProperty("Username")]
    [RelatedEntityExclusions("*")]
    [EntitySettings(Group = "Audit")]
    public class AuthenticationAttempt : AuditableEntity<long>, IAuthenticationAttempt
    {
        [Required]
        [StringLength(450)]
        public string Username { get; set; }
        [Required]
        public string Result { get; set; }
        [StringLength(39)]
        public string IpAddress { get; set; }
        public bool Ignore { get; set; }
        [IgnoreTrim]
        public string Message { get; set; }
        [StringLength(100)]
        public string AuthenticationPlugin { get; set; }
    }
}
