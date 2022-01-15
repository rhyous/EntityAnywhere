using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// Represents the Seed data necessary for a <see cref="EntityGroup"/>
    /// </summary>
    public class EntityGroupSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<EntityGroup>()
        {
            new EntityGroup()
            {
                Id = 1,
                Name = "Miscellaneous"
            },
            new EntityGroup()
            {
                Id = 2,
                Name = "Entity Management"
            },
            new EntityGroup()
            {
                Id = 3,
                Name = "Extension Entities"
            },

            new EntityGroup()
            {
                Id = 4,
                Name = "User Management"
            },
            new EntityGroup()
            {
                Id = 5,
                Name = "System Configuration"
            },
            new EntityGroup()
            {
                Id = 6,
                Name = "Auditing"
            }
        }.ToList<object>();
    }
}
