using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class ClaimConfigurationSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<ClaimConfiguration>
        {
            new ClaimConfiguration { Id = 1, Domain = nameof(User), Name = nameof(User.Id), Entity = nameof(User), EntityProperty = nameof(User.Id)},
            new ClaimConfiguration { Id = 2, Domain = nameof(User), Name = nameof(User.Username), Entity = nameof(User), EntityProperty = nameof(User.Username),},
            new ClaimConfiguration { Id = 3, Domain = nameof(UserRole), Name = "Role", Entity = nameof(UserRole), EntityProperty = nameof(UserRole.Name), EntityIdProperty = nameof(UserRole.Id), RelatedEntityIdProperty = nameof(UserRole) },
            new ClaimConfiguration { Id = 4, Domain = nameof(UserRole), Name = nameof(UserRole.LandingPageId), Entity = nameof(UserRole), EntityProperty = nameof(UserRole.LandingPageId), EntityIdProperty = nameof(UserRole.Id), RelatedEntityIdProperty = nameof(UserRole) }
        }.ToList<object>();        
    }
}