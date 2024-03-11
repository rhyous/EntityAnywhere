using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IItem : IBaseEntity<int>
    {
        string Name { get; set; }
    }
}
