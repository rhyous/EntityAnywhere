using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Tests
{
    [AlternateKey("Name")]
    [DisplayNameProperty("Name")]
    [EntitySettings(Description = "A test entity with a string Id.", Group = "TestGroup")]
    public class EntityString : IEntityString
    {
        [EntityProperty(Order = 1)]
        public string Id { get; set; }
        [IgnoreTrim]
        [EntityProperty(Order = 2)]
        public string Name { get; set; }
        [HRef]
        [EntityProperty(Order = 3)]
        public string Url { get; set; }
    }
}
