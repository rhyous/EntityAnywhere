using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    public class TestA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RelatedEntity(nameof(TestB), AllowedNonExistentValue = -1)]
        public int BId { get; set; }
    }
}
