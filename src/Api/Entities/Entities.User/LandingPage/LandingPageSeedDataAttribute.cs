using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class LandingPageSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<LandingPage>()
        {
            new LandingPage { Id = 1, Name = "Admin", Enabled = true}
        }.ToList<object>();
    }
}