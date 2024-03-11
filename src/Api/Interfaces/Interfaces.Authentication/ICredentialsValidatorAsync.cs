using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The contract that all credential validator plugins must implement.
    /// </summary>
    public interface ICredentialsValidatorAsync
    {
        /// <summary>
        /// The name of the CredentialsValidator plugin
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Take in credentials and verifies them against the validator.
        /// </summary>
        /// <param name="creds">An ICredentials object containing the username and password.</param>
        /// <returns>A token that can be used for subsequent communication after authentication.</returns>
        Task<CredentialsValidatorResponse> IsValidAsync(ICredentials creds);
    }
}