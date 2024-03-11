using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserTypeSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<UserType>()
        {
            new UserType { Id = 1, Type = "System" },
            new UserType { Id = 2, Type = "Internal" },
            new UserType { Id = 3, Type = "Partner" },
            new UserType { Id = 4, Type = "Customer" },
            new UserType { Id = 5, Type = "Organization" }
        }.ToList<object>();
    }
}
