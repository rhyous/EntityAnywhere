using Rhyous.Odata;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public class Token : BaseEntity<long>, IToken
    {
        [RelatedEntity("User", AutoExpand = true)]
        public long UserId { get; set; }
        public string Text { get; set; }
    }
}