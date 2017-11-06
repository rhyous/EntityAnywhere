using Rhyous.Odata;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    public class Token : Entity<int>, IToken
    {
        [RelatedEntity("User", AutoExpand = true)]
        public int UserId { get; set; }
        public string Text { get; set; }
    }
}