using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IAltKeyEntity : IBaseEntity<int>, IName, IDescription
    {
    }
}