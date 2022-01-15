using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;


namespace Rhyous.EntityAnywhere.Services.Tests
{
    [AlternateKey("Name")]
    [RelatedEntityExclusions("*")]
    public class EntityIntNullable : IEntityIntNullable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OptionalId { get; set; }
    }
}
