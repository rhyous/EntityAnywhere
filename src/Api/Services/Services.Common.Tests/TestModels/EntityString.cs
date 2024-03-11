using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [AlternateKey("Name")]
    public class EntityString : IEntityString
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
