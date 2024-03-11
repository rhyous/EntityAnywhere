using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserRoleEntityMapSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<UserRoleEntityMap>()
        {
            new UserRoleEntityMap { Id = 1, UserRoleId = 1, EntityId = 0 }
        }.ToList<object>();
    }
}
