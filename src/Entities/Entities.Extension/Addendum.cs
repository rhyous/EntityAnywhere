using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// This entity is used for extending any entity without creating a new property for that entity.
    /// Every entity has the ability to have Addenda, which is a custom property value list.
    /// </summary>
    [ExtensionEntity(AutoExpand = true)]
    [RelatedEntityExclusions("Addendum", "AlternateId")]
    public partial class Addendum : ExtensionEntity, IAddendum
    {
        [Required]
        [DistinctProperty("ExtensionEntityGroup")]
        [MaxLength(100)]
        public override string Value { get => base.Value; set => base.Value = value; }
    }
}
