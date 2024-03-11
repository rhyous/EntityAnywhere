using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;


namespace Rhyous.EntityAnywhere.Interfaces.Tests
{
    [AlternateKey("Name")]
    [RelatedEntityExclusions("*")]
    public class EntityIntNullable : IEntityIntNullable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? OptionalId { get; set; }
    }

    public interface IEntityIntNullable
    {
        int Id { get; set; }
        string Name { get; set; }
        int? OptionalId { get; set; }
    }
}
