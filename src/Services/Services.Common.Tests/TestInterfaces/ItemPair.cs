using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IItemPair : IBaseEntity<int>
    {
        string Name { get; set; }
    }
}
