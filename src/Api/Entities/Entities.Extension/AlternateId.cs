using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// This entity is used for extending any entity with an alternate identifier without creating
    /// a new property for that entity.
    /// Every entity has the ability to have Alternate Ids, which is a custom property value list.
    /// </summary>
    /// <remarks>This is only different from <see cref="Addendum"/> in that it is specific for identifiers, usually from other systems.</remarks>
    [ExtensionEntity(AutoExpand = true)]
    [RelatedEntityExclusions("*")]
    public class AlternateId : ExtensionEntity, IAlternateId
    {
        /// <inheritdoc />
        [StringLength(100)]
        public override string Value { get => base.Value; set => base.Value = value; }
    }
}