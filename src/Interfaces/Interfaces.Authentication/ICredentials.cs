namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The username and password used to authenticate.
    /// </summary>
    public partial interface ICredentials
    {
        /// <summary>
        /// The username used for authentication
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// The password used for authentication
        /// </summary>
        string Password { get; set; }
    }
}
