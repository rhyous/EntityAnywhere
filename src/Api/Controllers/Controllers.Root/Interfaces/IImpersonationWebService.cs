using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IImpersonationWebService
    {
        /// <summary>
        /// Calls Impersonate to impersonate a customer
        /// </summary>
        /// <returns>Token impersonating a different customer</returns>
        Task<Token> ImpersonateAsync(string roleId);
    }
}
