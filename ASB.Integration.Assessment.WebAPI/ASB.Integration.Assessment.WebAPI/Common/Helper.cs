using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ASB.Integration.Assessment.WebAPI.Common
{
    /// <summary>
    /// Helper class.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Hash string.
        /// </summary>
        /// <param name="stringToHash">String to hash.</param>
        /// <param name="secretKey">Salt.</param>
        /// <returns>Hash Value.</returns>
        public static string HashString(string stringToHash, string secretKey)
        {
            stringToHash += secretKey;

            using var sha = new SHA512CryptoServiceProvider();
            var hashArray = sha.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            var sBuilder = new StringBuilder();
            for (int index = 0; index < hashArray.Length; index++)
            {
                sBuilder.Append(hashArray[index].ToString("X2", CultureInfo.InvariantCulture));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Encrypt data. (using to encrypt card number)
        /// </summary>
        /// <param name="clearText">Clear text.</param>
        /// <returns>Encrypted data.</returns>
        public static string Encrypt(string clearText)
        {
            if (string.IsNullOrWhiteSpace(clearText))
            {
                throw new ArgumentException($"'{nameof(clearText)}' cannot be null or whitespace", nameof(clearText));
            }

            return new Encryption(null).Encrypt(clearText);
        }

        /// <summary>
        /// Decrypt. (used to decrypt card number)
        /// </summary>
        /// <param name="encryptedText">Encrypted text.</param>
        /// <returns>Clear text.</returns>
        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
            {
                throw new System.ArgumentException($"'{nameof(encryptedText)}' cannot be null or whitespace", nameof(encryptedText));
            }

            return new Encryption(null).Decrypt(encryptedText);
        }
    }
}
