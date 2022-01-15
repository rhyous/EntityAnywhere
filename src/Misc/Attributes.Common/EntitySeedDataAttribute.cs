using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// Inherit this class and decorate an entity to provide seed data.
    /// </summary>
    public abstract class EntitySeedDataAttribute : EntityAttribute
    {
        public abstract List<object> Objects { get; }
    }
}
