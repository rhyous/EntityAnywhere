using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IUserRoleEntityDataCacheFactory
    {
        IUserRoleEntityDataCache Cache { get; set; }
        Task UpdateRoleEntityDataAsync(int roleId);
    }
}