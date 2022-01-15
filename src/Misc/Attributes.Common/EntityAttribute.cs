using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// All attributes that can be added to an Entity should inherit this attribute.
    /// </summary>
    public class EntityAttribute : Attribute
    {
        public virtual bool CanGenerateRepository { get; set; } = true; // Default to true
    }
}