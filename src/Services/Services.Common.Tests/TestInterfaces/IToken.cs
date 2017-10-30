using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    public interface IToken : IEntity<int>
    {
        string Text { get; set; }
    }
}
