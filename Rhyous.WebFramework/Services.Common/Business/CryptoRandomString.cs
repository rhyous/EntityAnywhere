using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Rhyous.WebFramework.Services
{
    public class CryptoRandomString
    {
        public const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
        public const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Numbers = "0123456789";
        public const string AlphaNumeric = LowerCase + UpperCase + Numbers;
        public static string AlphaNumericNonAmbigous
        {
            get { return _AlphaNumericNonAmbiguous ?? (_AlphaNumericNonAmbiguous = new Regex("0|O|1|l").Replace(LowerCase + UpperCase + Numbers, "")); }
        } private static string _AlphaNumericNonAmbiguous;

        public static string GetCryptoRandomBase64String(int length)
        {
            var buffer = new byte[length];
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetNonZeroBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        public static string GetCryptoRandomBaseNString(int length, byte baseN, int offset = 32, char[] allowedCharacters = null)
        {
            var buffer = new byte[length];
            var builder = new StringBuilder();

            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetBytes(buffer);
                foreach (var b in buffer)
                {
                    var tmpbuff = new byte[] { b };
                    int max = (baseN * (256 / baseN)) - 1; // minus 1 because we start at 0
                    while (tmpbuff[0] > max)
                    {
                        rngCryptoServiceProvider.GetBytes(tmpbuff);
                    }
                    var singleChar = (allowedCharacters == null)
                        ? ByteToBaseNChar(tmpbuff[0], baseN, offset) // Start at ascii 32 (space) by default
                        : ByteToAllowedCharacter(tmpbuff[0], allowedCharacters);
                    builder.Append(singleChar);
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Any character from ASCII decimal 32 (space), including alphanumeric and punctuation, 
        /// to ASCII decimal 126 (~).
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetCryptoRandomBase95String(int length)
        {
            return GetCryptoRandomBaseNString(length, 95);
        }

        /// <summary>
        /// A random string made up of Alphanumeric characters.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="removeAmbiguous"></param>
        /// <returns></returns>
        public static string GetCryptoRandomAlphaNumericString(int length, bool removeAmbiguous = false)
        {
            return GetCryptoRandomBaseNString(length, (byte)AlphaNumeric.Length, 0, removeAmbiguous
                ? AlphaNumeric.ToCharArray()
                : AlphaNumericNonAmbigous.ToCharArray());
        }

        public static char ByteToBaseNChar(byte b, int baseN, int asciiOffset)
        {
            return (char)(b % baseN + asciiOffset);
        }

        public static char ByteToAllowedCharacter(byte b, char[] allowedCharacters)
        {
            return allowedCharacters[(b % allowedCharacters.Length)];
        }
    }
}
