using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    [AlternateKey("Name")]
    public class EntityInt : IEntityInt
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
