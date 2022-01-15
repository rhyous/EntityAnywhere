using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IEntityInt : IBaseEntity<int>, IName
    {
        long LongNumber { get; set; }
        string ReadOnlyText { get; }
    }
}