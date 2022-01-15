using Rhyous.WebFramework.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Entities
{
    public class OrganizationSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<Organization>
        {
            new Organization { Name = "Internal", Description = "Your company." }
        }.ToList<object>();
    }
}
