using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserTypeMapMapSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<UserTypeMap>()
        {
            new UserTypeMap { Id = 1, UserId = 1, UserTypeId = 1 },
            new UserTypeMap { Id = 2, UserId = 2, UserTypeId = 1 },
            new UserTypeMap { Id = 3, UserId = 3, UserTypeId = 2 }
        }.ToList<object>();
    }
}
