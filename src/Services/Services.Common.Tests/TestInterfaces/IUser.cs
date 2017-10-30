using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services.Common.Tests
{
    public interface IUser : IEntity<int>, IName
    {
        int UserTypeId { get; set; }
    }
}
