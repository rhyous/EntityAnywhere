namespace Rhyous.WebFramework.Services.Security
{
    /// <summary>
    /// Responsible for handling encrpytion and decrpytion algorithms
    /// </summary>
    public interface IPasswordSecurity
    {
        /// <summary>
        /// Encrypt some plain text
        /// </summary>
        /// <param name="plainText">The data to encrpyt</param>
        /// <returns>A hash</returns>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypt a hashed string
        /// </summary>
        /// <param name="encryptedText">The encrpypted text.</param>
        /// <returns>A decrypted plain text string.</returns>
        string Decrypt(string encryptedText);

        /// <summary>
        /// Returns true if the plain text matches the encrypted text.
        /// </summary>
        /// <param name="plainText">A string to compare.</param>
        /// <param name="encryptedText">An encrypted string.</param>
        /// <returns>true if the encrypted text is the same as the plain text.</returns>
        /// <remarks>This check should encrypt and compare, not decrypt and compare.</remarks>
        bool Compare(string plainText, string encryptedText);
    }
}
