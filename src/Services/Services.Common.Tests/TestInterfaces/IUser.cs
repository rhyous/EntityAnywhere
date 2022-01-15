using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Services.Common.Tests
{
    public interface IUser : IBaseEntity<int>, IName
    {
        int UserTypeId { get; set; }
    }
}
