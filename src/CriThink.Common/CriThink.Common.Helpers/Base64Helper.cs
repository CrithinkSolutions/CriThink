using System;
using System.Text;

namespace CriThink.Common.Helpers
{
    public sealed class Base64Helper
    {
        /// <summary>
        /// Encode the provided string in base64
        /// </summary>
        /// <param name="plain">String to encode</param>
        /// <returns>String encoded</returns>
        public static string ToBase64(string plain)
        {
            if (string.IsNullOrWhiteSpace(plain)) return string.Empty;

            var bytes = Encoding.UTF8.GetBytes(plain);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decode the provided base64 string
        /// </summary>
        /// <param name="encoded">The base64 encoded string</param>
        /// <returns>String decoded</returns>
        public static string FromBase64(string encoded)
        {
            if (string.IsNullOrWhiteSpace(encoded)) return string.Empty;

            var bytes = Convert.FromBase64String(encoded);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
