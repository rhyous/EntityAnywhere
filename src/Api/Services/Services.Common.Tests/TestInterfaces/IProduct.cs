using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IProduct : IBaseEntity<int>
    {
        string Name { get; set; }
        string Version { get; set; }
        int Type { get; set; } 
    }
}
