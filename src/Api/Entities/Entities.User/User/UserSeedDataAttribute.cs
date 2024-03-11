using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<User>
        {
            new User { Id = 1, Username = "system", Enabled = true },
            new User { Id = 2, Username = "unknown" },
            new User { Id = 3, Username = "admin", Password = "@dmin1234!", IsHashed = false, Enabled = true }
        }.ToList<object>();
    }
}