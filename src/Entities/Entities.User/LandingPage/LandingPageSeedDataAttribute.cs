using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class LandingPageSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<LandingPage>()
        {
            new LandingPage { Id = 1, Name = "Admin", Enabled = true},
            new LandingPage { Id = 2, Name = "Teacher", Enabled = true },
            new LandingPage { Id = 3, Name = "Student", Enabled = true },
            new LandingPage { Id = 4, Name = "Parent", Enabled = true }
        }.ToList<object>();
    }
}