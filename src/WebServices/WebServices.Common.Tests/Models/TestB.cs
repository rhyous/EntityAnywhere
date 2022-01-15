using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    public class TestB
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity(nameof(TestA))]
        public int AId { get; set; }
    }
}
