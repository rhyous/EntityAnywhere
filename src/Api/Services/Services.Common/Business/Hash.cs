using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// This allows for easily created hashes from MD5, SHA1, SHA256, and SHA512.
    /// </summary>
    public class Hash
    {
        public static Encoding DefaultEncoding = Encoding.UTF8;
        public const HashType DefaultHashType = HashType.SHA256;

        public enum HashType
        {
            MD5,
            SHA1,
            SHA256,
            SHA512
        }

        /// <summary>
        /// Gets a hash of a string using the specified hashtype and encoding.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <param name="hashType">The hash type to use: MD5, SHA1, SHA256, or SHA512.</param>
        /// <param name="encoding">The encoding to use. If the encoding is not specified, then the encoding set in DefaultEncoding is used.</param>
        /// <returns>A hash of a string.</returns>
        public static string Get(string text, HashType hashType = DefaultHashType, Encoding encoding = null)
        {
            switch (hashType)
            {
                case HashType.MD5:
                    return Get(text, new MD5CryptoServiceProvider(), encoding);
                case HashType.SHA1:
                    return Get(text, new SHA1Managed(), encoding);
                case HashType.SHA256:
                    return Get(text, new SHA256Managed(), encoding);
                case HashType.SHA512:
                    return Get(text, new SHA512Managed(), encoding);
                default:
                    throw new CryptographicException("Invalid hash alrgorithm.");
            }
        }

        /// <summary>
        /// Gets a hash of a salted string using the specified hashtype and encoding.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <param name="salt">the salt to append to the string before hashing it.</param>
        /// <param name="hashType">The hash type to use: MD5, SHA1, SHA256, or SHA512.</param>
        /// <param name="encoding">The encoding to use. If the encoding is not specified, then the encoding set in DefaultEncoding is used.</param>
        /// <returns>A hash of a salted string.</returns>
        public static string Get(string text, string salt, HashType hashType = DefaultHashType, Encoding encoding = null)
        {
            return Get(text + salt, hashType, encoding);
        }

        /// <summary>
        /// Gets a hash of a salted string using the specified HashAlgorithm and encoding.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <param name="algorithm">The HashAlgrithm to use.</param>
        /// <param name="encoding">The encoding to use. If the encoding is not specified, then the encoding set in DefaultEncoding is used.</param>
        /// <returns>A hash of a string.</returns>
        public static string Get(string text, HashAlgorithm algorithm, Encoding encoding = null)
        {
            byte[] message = (encoding == null) ? DefaultEncoding.GetBytes(text) : encoding.GetBytes(text);
            byte[] hashValue = algorithm.ComputeHash(message);
            return hashValue.Aggregate(string.Empty, (current, x) => current + string.Format("{0:x2}", x));
        }

        /// <summary>
        /// Compares a known hash to a generated hash.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <param name="hashString">The known hash.</param>
        /// <param name="hashType">The hash type to use: MD5, SHA1, SHA256, or SHA512.</param>
        /// <param name="encoding">The encoding to use. If the encoding is not specified, then the encoding set in DefaultEncoding is used.</param>
        /// <returns>True if the known hash matches the hash of the text.</returns>
        public static bool Compare(string text, string hashString, HashType hashType = DefaultHashType, Encoding encoding = null)
        {
            string originalHash = Get(text, hashType, encoding);
            return (originalHash == hashString);
        }

        /// <summary>
        /// Compares a known hash to a generated hash.
        /// </summary>
        /// <param name="text">The string to hash.</param>
        /// <param name="salt">the salt to append to the string before hashing it.</param>
        /// <param name="hashString">The known hash.</param>
        /// <param name="hashType">The hash type to use: MD5, SHA1, SHA256, or SHA512.</param>
        /// <param name="encoding">The encoding to use. If the encoding is not specified, then the encoding set in DefaultEncoding is used.</param>
        /// <returns>True if the known hash matches the hash of the text.</returns>
        public static bool Compare(string text, string salt, string hashString, HashType hashType = DefaultHashType, Encoding encoding = null)
        {
            return Compare(text + salt, hashString, hashType, encoding);
        }
    }
}