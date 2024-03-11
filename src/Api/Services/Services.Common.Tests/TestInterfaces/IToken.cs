using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IToken : IBaseEntity<long>
    {
        string Text { get; set; }
    }
}
