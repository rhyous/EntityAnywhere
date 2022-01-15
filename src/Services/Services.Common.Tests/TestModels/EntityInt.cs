using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [AlternateKey("Name")]
    [RelatedEntityExclusions("*")]
    public class EntityInt : IEntityInt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long LongNumber { get; set; }
        public string ReadOnlyText { get; }
    }
}
