using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    public interface IEntityString : IBaseEntity<string>, IName
    {
    }

    [DisplayNameProperty("Name")]
    [AlternateKey("Name")]
    public class EntityString : IEntityString
    {
        public string Id { get; set; }
        [IgnoreTrim]
        public string Name { get; set; }
    }
}