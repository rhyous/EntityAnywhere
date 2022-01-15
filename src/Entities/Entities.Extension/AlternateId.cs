using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    [ExtensionEntity(AutoExpand = true)]
    [RelatedEntityExclusions("Addendum", "AlternateId")]
    public class AlternateId : ExtensionEntity, IAlternateId
    {
        [MaxLength(100)]
        public override string Value { get => base.Value; set => base.Value = value; }
    }
}