using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    [DisplayNameProperty("Name")]
    [AlternateKey("Name")]
    public class EntityString : IEntityString
    {
        public string Id { get; set; }
        [IgnoreTrim]
        public string Name { get; set; }
    }
}
