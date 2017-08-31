namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The contract that all credential validator plugins must implement.
    /// </summary>
    public interface ICredentialsValidator
    {
        /// <summary>
        /// Take in credentials and verifies them against the validator.
        /// </summary>
        /// <param name="creds">An ICredentials object containing the username and password.</param>
        /// <param name="token">A token that can be used for subsequent communication after authentication.</param>
        /// <returns>True of false, whether the credentials are valid.</returns>
        bool IsValid(ICredentials creds, out IToken token);
    }
}