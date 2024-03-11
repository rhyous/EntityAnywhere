using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [AlternateKey("Name")]
    [RelatedEntityExclusions("AlternateId")]
    public class EntityInt : IEntityInt
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
