using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [AlternateKey("Name")]
    public class AltKeyEntity : BaseEntity<int>, IAltKeyEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}