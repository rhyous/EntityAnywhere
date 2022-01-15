using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// Responsible for deciding whether or not an account is locked and to log an authentication attempt
    /// </summary>
    public interface IAccountLocker
    {
        /// <summary>
        /// Add an Authentication Attempt
        /// </summary>
        /// <param name="attempt">The Authentication Attempt</param>
        Task AddAttempt(AuthenticationAttempt attempt);

        /// <summary>
        /// Determine if an account is locked
        /// </summary>
        /// <param name="creds">The credentials to check</param>
        /// <returns>True if the account is locked</returns>
        Task<bool> IsLocked(Credentials creds);
    }
}