using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Rhyous.EntityAnywhere.Services.Security

{
    /// <summary>
    /// Responsible for hashing algorithms using the <see cref="RijndaelManaged"/> algorithm
    /// </summary>
    public class RijndaelManagedSecurity : IPasswordSecurity
    {
        #region Privates

        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int keyAndBlockSize = 256;
        private const int keyAndBlockSizeInBytes = keyAndBlockSize / 8;
        private const CipherMode cipherMode = CipherMode.CBC;
        private const PaddingMode paddingMode = PaddingMode.PKCS7;

        private IPasswordSecuritySettings settings;

        #endregion

        #region Constructor

        /// <summary>
        /// Construct a new <see cref="RijndaelManagedSecurity"/> with the required settings
        /// </summary>
        /// <param name="settings">The password security settings</param>
        public RijndaelManagedSecurity(IPasswordSecuritySettings settings)
        {
            this.settings = settings;
        }

        #endregion

        ///<inheritdoc/>
        public string Decrypt(string hash)
        {
            return Decrypt(hash, settings.CipherKey);
        }

        ///<inheritdoc/>
        public string Encrypt(string plainText)
        {
            return Encrypt(plainText, settings.CipherKey);
        }

        public bool Compare(string plainText, string encryptedText)
        {
            var cypherData = GetCypherData(encryptedText);
            cypherData.Text = Encoding.UTF8.GetBytes(plainText);
            var encryptedTextInput = Encrypt(settings.CipherKey, cypherData);
            return encryptedTextInput == encryptedText;
        }

        /// <summary>
        /// Encrypt a string using the provided cypherKey
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="cypherKey"></param>
        /// <returns></returns>
        private string Encrypt(string plainText, string cypherKey)
        {
            if (string.IsNullOrEmpty(plainText)) plainText = Guid.NewGuid().ToString();


            var cypherData = new CypherData
            {
                // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
                // so that the same Salt and IV values can be used when decrypting.  
                Salt = Generate256BitsOfRandomEntropy(),
                IV = Generate256BitsOfRandomEntropy(),
                Text = Encoding.UTF8.GetBytes(plainText)
            };
            return Encrypt(cypherKey, cypherData);
        }

        private string Encrypt(string cypherKey, CypherData cypherData)
        {
            using (var password = new Rfc2898DeriveBytes(cypherKey, cypherData.Salt, settings.DerivationIterations))
            {
                var keyBytes = password.GetBytes(keyAndBlockSizeInBytes);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = keyAndBlockSize;
                    symmetricKey.Mode = cipherMode;
                    symmetricKey.Padding = paddingMode;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, cypherData.IV))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(cypherData.Text, 0, cypherData.Text.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = cypherData.Salt;
                                cipherTextBytes = cipherTextBytes.Concat(cypherData.IV).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        private string Decrypt(string cipherText, string cipherKey)
        {

            var cypherData = GetCypherData(cipherText);
            using (var password = new Rfc2898DeriveBytes(cipherKey, cypherData.Salt, settings.DerivationIterations))
            {
                var keyBytes = password.GetBytes(keyAndBlockSizeInBytes);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = keyAndBlockSize;
                    symmetricKey.Mode = cipherMode;
                    symmetricKey.Padding = paddingMode;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, cypherData.IV))
                    {
                        using (var memoryStream = new MemoryStream(cypherData.Text))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cypherData.Text.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static CypherData GetCypherData(string cipherText)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cypherBytes = Convert.FromBase64String(cipherText);
            var cypherData = new CypherData
            {
                // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
                Salt = cypherBytes.Take(keyAndBlockSizeInBytes).ToArray(),
                // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
                IV = cypherBytes.Skip(keyAndBlockSizeInBytes).Take(keyAndBlockSizeInBytes).ToArray(),
                // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
                Text = cypherBytes.Skip(keyAndBlockSizeInBytes * 2).Take(cypherBytes.Length - (keyAndBlockSizeInBytes * 2)).ToArray()
            };
            return cypherData;
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[keyAndBlockSizeInBytes]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        internal class CypherData
        {
            public byte[] Text { get; set; }
            public byte[] Salt { get; set; }
            public byte[] IV { get; set; }
        }
    }
}
