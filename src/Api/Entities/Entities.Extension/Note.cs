using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// This entity is used for extending any entity with notes without creating a new property
    /// on that entity for notes. Every entity has the ability to have Notes, which is a custom 
    /// property-value list.
    /// </summary>
    [ExtensionEntity(AutoExpand = false)]
    [RelatedEntityExclusions("*")]
    public class Note : ExtensionEntity, INote
    {
    }
}