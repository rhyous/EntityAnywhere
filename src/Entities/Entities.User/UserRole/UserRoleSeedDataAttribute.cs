using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class UserRoleSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<UserRole>()
        {
            new UserRole { Id = 1, Name = "Admin", Enabled = true, LandingPageId = 1 },
            new UserRole { Id = 2, Name = "Teacher", Enabled = true, LandingPageId = 2 },
            new UserRole { Id = 3, Name = "Student", Enabled = true, LandingPageId = 3 },
            new UserRole { Id = 4, Name = "Parent", Enabled = true , LandingPageId = 4 }
        }.ToList<object>();
    }
}
