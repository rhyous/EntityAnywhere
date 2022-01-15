using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserRoleMembershipSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<UserRoleMembership>()
        {
            new UserRoleMembership { Id = 1, UserId = 1, UserRoleId = 1 },
            new UserRoleMembership { Id = 2, UserId = 3, UserRoleId = 1 }
        }.ToList<object>();
    }
}
