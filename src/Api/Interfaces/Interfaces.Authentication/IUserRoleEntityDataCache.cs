using Rhyous.Collections;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The cache of valid user roles.
    /// </summary>
    /// <remarks>
    /// WARNING! The factory that creates this has to make a call to the UserRole web service, so you cannot
    /// inject this into any object registered per call, otherwise, you could create an infinite loop.
    /// </remarks>
    public interface IUserRoleEntityDataCache : ICacheBase<ConcurrentDictionaryWrapper<int, IUserRoleEntityData>>
    {
        ConcurrentDictionary<string, int> UserRoleIds { get; }
        IUserRoleEntityData this[string key] { get; }
        bool TryGetValue(string key, out IUserRoleEntityData value);
        bool TryGetValue(int key, out IUserRoleEntityData value);
        Task UpdateRoleEntityDataAsync(int roleId);
        bool Remove(int roleId);
    }
}