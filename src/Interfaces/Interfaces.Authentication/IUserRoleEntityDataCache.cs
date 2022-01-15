using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The cache of valid user roles.
    /// </summary>
    /// <remarks>
    /// WARNING! The factory that creates this has to make a call to the UserRole web service, so you cannot
    /// inject this into any object registered per call, otherwise, you could create an infinite loop.
    /// </remarks>
    public interface IUserRoleEntityDataCache : IDictionary<int, IUserRoleEntityData>
    {
        IDictionary<string, int> UserRoleIds { get; }
        IUserRoleEntityData this[string key] { get; }
        bool TryGetValue(string key, out IUserRoleEntityData value);
    }
}