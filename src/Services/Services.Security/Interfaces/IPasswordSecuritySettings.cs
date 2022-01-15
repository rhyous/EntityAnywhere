namespace Rhyous.WebFramework.Services.Security
{
    /// <summary>
    /// Represents the settings needed for configuring password security
    /// </summary>
    public interface IPasswordSecuritySettings
    {
        /// <summary>
        /// The cypher key. 
        /// </summary>
        string CipherKey { get; }

        /// <summary>
        /// How many iterations to go through to encrypt/decrpyt the password
        /// </summary>
        int DerivationIterations { get; }
    }
}
