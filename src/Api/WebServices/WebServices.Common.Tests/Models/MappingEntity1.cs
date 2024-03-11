using Rhyous.Odata;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    public class MappingEntity1 : IMappingEntity1
    {
        public int Id { get; set; }

        [RelatedEntity(nameof(EntityBasic))]
        public int EntityBasicId { get; set; }

        [RelatedEntity(nameof(EntityBasic))]
        public int EntityIntId { get; set; }
    }
}
