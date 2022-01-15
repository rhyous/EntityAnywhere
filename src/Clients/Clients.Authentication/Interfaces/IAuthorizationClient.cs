using Rhyous.EntityAnywhere.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IAuthorizationClient
    {
        Task<UserRoleEntityData> GetRoleDataAsync();
    }
}