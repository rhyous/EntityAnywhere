using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// The UserRole entity. This should be used to put users in Roles and assign UserRoles some authorization claims.
    /// </summary>
    [LandingPageSeedData]
    [AlternateKey("Name")]
    [RelatedEntityForeign(nameof(UserRole), nameof(LandingPage))]
    public partial class LandingPage : AuditableEntity<int>, ILandingPage
    {
        /// <inheritdoc />
        [Required]
        public string Name { get; set; }
        /// <inheritdoc />
        [IgnoreTrim]
        public string Description { get; set; }

        /// <inheritdoc />
        public bool Enabled { get; set; }
    }
}