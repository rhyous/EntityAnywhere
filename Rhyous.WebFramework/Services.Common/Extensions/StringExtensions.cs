using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Rhyous.WebFramework.Services
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// Converts a string to an int.
        /// </summary>
        /// <param name="s">String Input</param>
        /// <returns></returns>
        public static int ToInt(this string s)
        {
            int convertedValue;
            int.TryParse(s, out convertedValue);
            return convertedValue;
        }

        /// <summary>
        /// Gets as double.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static double GetAsDouble(this string s)
        {
            try
            {
                s = s.Trim();
                return String.IsNullOrWhiteSpace(s) ? 0.0 : Convert.ToDouble(s);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Removes diacritics from a string.
        /// </summary>
        /// <param name="stIn"></param>
        /// <returns>string without diacitics</returns>
        public static string RemoveDiacritics(this string inString)
        {
            var strFormD = inString.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in strFormD.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                sb.Append(c);
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static Stream ToStream(this string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8; // Default
            }
            return new MemoryStream(encoding.GetBytes(str ?? ""));
        }

        public static string AsString(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8; // Default
            }
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static string Wrap(this string s, char prefix, char postfix)
        {
            if (!string.IsNullOrWhiteSpace(s))
            {
                if (s[0] != prefix)
                    s = prefix + s;
                if (s[s.Length - 1] != postfix)
                    s += postfix;
            }
            return s;
        }
    }
}
