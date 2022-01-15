using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public class ItemPair : BaseEntity<int>, IItemPair
    {
        public string Name { get; set; }

        [RelatedEntity("Item")]
        public int ItemId { get; set; }

        [RelatedEntity("Item", RelatedEntityAlias = "OtherItem")]
        public int OtherItemId { get; set; }
    }
}
