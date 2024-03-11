using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserRoleSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => GetObjects();
        public List<object> GetObjects()
        {
            int i = 0;
            return new List<UserRole>()
            {
                new UserRole { Id = ++i, Name = "Admin", Enabled = true, LandingPageId = i },
            }.ToList<object>();
        }
    }
}
