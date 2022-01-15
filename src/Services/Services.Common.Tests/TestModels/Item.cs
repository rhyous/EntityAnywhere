using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    [RelatedEntityForeign("ItemPair", "Item")]
    [RelatedEntityForeign("ItemPair", "Item", RelatedEntityAlias = "OtherItemPair", ForeignKeyProperty = "OtherItemId")]
    public class Item : BaseEntity<int>, IItem
    {
        public string Name { get; set; }
    }
}