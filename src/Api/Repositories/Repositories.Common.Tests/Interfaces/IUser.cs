using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    public interface IUser : IBaseEntity<int>, IName
    {
    }
}
