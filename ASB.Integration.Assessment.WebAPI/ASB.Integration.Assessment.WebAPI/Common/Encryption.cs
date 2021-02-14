using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ASB.Integration.Assessment.WebAPI.Common
{
    /// <summary>
    /// Represents an object that encrypts and decrypts data using a shared key and the AES algorithm.  Manages the creation of the Initialization Vector (IV) and using a HMAC to validate encrypted data.  Returns the IV and HMAC as part of the results to simplify usage.
    /// </summary>
    internal class Encryption
    {
        // The IV must always be the same size as the block size.
        // For AES the block size must always be 128 bits (byte[16])
        private const int BlockSizeBytes = 16;
        private const int IVSizeBytes = BlockSizeBytes;

        // The key size for the AES encyptor is 128 bits (16 bytes)
        private const int EncryptionSizeBytes = 16;

        // The key size for the HMAC is 256 bits (32 bytes)
        private const int HmacKeySizeBytes = 32;

        // The output of the HMAC is 256 bits (32 bytes)
        private const int HmacSizeBytes = 32;

        private readonly byte[] _encryptionKey;
        private readonly byte[] _hmacKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="Encryption"/> class.
        /// </summary>
        /// <param name="key">The shared encryption key.  Should be larger than 384 bits (48 bytes).</param>
        public Encryption(byte[] key)
        {
            if (key == null || key.Length == 0)
            {
                key = new byte[48] { 5, 246, 167, 252, 129, 209, 130, 79, 165, 130, 47, 71, 97, 247, 10, 177, 190, 56, 240, 49, 112, 12, 155, 70, 191, 101, 171, 82, 240, 55, 24, 102, 73, 207, 28, 157, 221, 225, 60, 78, 189, 82, 137, 121, 127, 189, 56, 191 };
            }

            byte[] hashedKey;

            // We need two keys, one for encryption and one for the HMAC.
            // We don't want to use the same key for each.
            // By using SHA384 on the provided key we get SHA384 bits.
            // We can use the first 128 bits for the encryption key and the
            // remaining 256 for the HMAC.
            using (var sha384 = SHA384.Create())
            {
                hashedKey = sha384.ComputeHash(key);
            }

            _encryptionKey = new byte[EncryptionSizeBytes];
            _hmacKey = new byte[HmacKeySizeBytes];

            Array.Copy(hashedKey, 0, _encryptionKey, 0, EncryptionSizeBytes);
            Array.Copy(hashedKey, EncryptionSizeBytes, _hmacKey, 0, HmacKeySizeBytes);
        }

        /// <summary>
        /// Encrypts a string using the AES algorithm.
        /// </summary>
        /// <param name="clearText">The input to the encryption operation.</param>
        /// <returns>The output of the encryption operation with a 16 <see langword="byte"/> Initialization Vector (IV) prefix and a 32 byte HMAC suffix.</returns>
        public string Encrypt(string clearText)
        {
            if (clearText == null)
            {
                throw new ArgumentNullException(nameof(clearText));
            }

            // Encrypt data
            var data = Encoding.Unicode.GetBytes(clearText);
            var encrypted = Encrypt(data);

            // Return as base64 string
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Decrypts a string that was previously encrypted with <see cref="Encryption"/>.
        /// </summary>
        /// <param name="cypherText">The input to the decryption operation including the 16 byte Initialization Vector (IV) prefix and a 32 byte HMAC suffix.</param>
        /// <returns>The decrypted string.</returns>
        public string Decrypt(string cypherText)
        {
            if (cypherText == null)
            {
                throw new ArgumentNullException(nameof(cypherText));
            }

            // Parse base64 string
            var data = Convert.FromBase64String(cypherText);

            // Decrypt data
            var decrypted = Decrypt(data);

            return Encoding.Unicode.GetString(decrypted);
        }

        /// <summary>
        /// Encrypts an array of bytes using the AES algorithm.
        /// </summary>
        /// <param name="clearBytes">The input to the encryption operation.</param>
        /// <returns>The output of the encryption operation with a 16 <see langword="byte"/> Initialization Vector (IV) prefix and a 32 byte HMAC suffix.</returns>
        public byte[] Encrypt(byte[] clearBytes)
        {
            // Implementation initially taken from https://security-code-scan.github.io/#SCS0013
            // with changes based on https://security.stackexchange.com/questions/63132/when-to-use-hmac-alongside-aes
            if (clearBytes is null)
            {
                throw new ArgumentNullException(nameof(clearBytes));
            }

            // The output should be the length of the input, one block of padding that is
            // added by the encryption, the IV and the length of the HMAC.
            // Adding on an extra block for good measure to ensure we don't have
            // to resize the internal buffer.
            // This should result in less memory having to be shuffled around
            // and ultimately better performance.
            var probableCypherStreamLength = clearBytes.Length + (IVSizeBytes * 3) + 32;

            using var cypherStream = new MemoryStream(probableCypherStreamLength);
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _encryptionKey;

            var iv = aes.IV;

            // Include the IV at the start of the output
            cypherStream.Write(iv, 0, IVSizeBytes);

            using var encrypter = aes.CreateEncryptor();
#pragma warning disable CA2000 // Dispose objects before losing scope. It is dispose in encryptor.
            var cryptoStream = new CryptoStream(cypherStream, encrypter, CryptoStreamMode.Write);
#pragma warning restore CA2000 // Dispose objects before losing scope

            cryptoStream.Write(clearBytes, 0, clearBytes.Length);
            cryptoStream.FlushFinalBlock();

            using (HMACSHA256 hmac = new HMACSHA256(_hmacKey))
            {
                // Get an HMAC of the encrypted contents and importantly the IV
                cypherStream.Seek(0, SeekOrigin.Begin);
                var tab = hmac.ComputeHash(cypherStream);

                cypherStream.Write(tab, 0, tab.Length);
            }

            return cypherStream.ToArray();
        }

        /// <summary>
        /// Decrypts an array of bytes that was previously encrypted with <see cref="Encryption"/>.
        /// </summary>
        /// <param name="encryptedBytes">The input to the decryption operation including the 16 byte Initialization Vector (IV) prefix and a 32 byte HMAC suffix.</param>
        /// <returns>The output of the decryption operation.</returns>
        public byte[] Decrypt(byte[] encryptedBytes)
        {
            if (encryptedBytes is null)
            {
                throw new ArgumentNullException(nameof(encryptedBytes));
            }

            // The byte array must be large enough to contain an IV, the HMAC and at least one block of data
            if (encryptedBytes.Length - HmacKeySizeBytes - IVSizeBytes - BlockSizeBytes < 0)
            {
                throw new CryptographicException("Encrypted data is too short");
            }

            // We must validate the HMAC first before doing any other cryptographic operations to avoid potential attacks.
            var providedHmac = new byte[HmacSizeBytes];
            using (var hmac = new HMACSHA256(_hmacKey))
            {
                Array.Copy(encryptedBytes, encryptedBytes.Length - HmacSizeBytes, providedHmac, 0, HmacSizeBytes);
            }

            using var algorithm = Aes.Create();
            var iv = new byte[IVSizeBytes];

            Array.Copy(encryptedBytes, 0, iv, 0, IVSizeBytes);

            algorithm.Key = _encryptionKey;
            algorithm.IV = iv;
            algorithm.Mode = CipherMode.CBC;
            algorithm.Padding = PaddingMode.PKCS7;

            // Declare the output stream to have a capacity of the encrypted length
            // This should be marginally too big for the data we'll write to it, but it will ensure
            // that we don't have to resize the internal buffer.
            // This should result in less memory having to be shuffled around
            // and ultimately better performance.
            using var clearStream = new MemoryStream(encryptedBytes.Length - HmacKeySizeBytes);
            using (var decryptor = algorithm.CreateDecryptor())
            {
                var encryptedByteLength = encryptedBytes.Length - HmacKeySizeBytes - IVSizeBytes;

                // I would love to not have to create this array, but despite my
                // best efforts I'm not able to get CryptoStream to not read the HMAC at the end of the stream.
                // I've tried limiting what the CrypoStream reads, but
                // in the dispose of CryptoStream it checks to see it has finished reading
                // from the buffer and if it hasn't it tries to read the last bytes.
                // I've tried manually overwriting the HMAC bytes with padding bytes, but I can't get that working.
                var cypherBytes = new byte[encryptedByteLength];

                Array.Copy(encryptedBytes, IVSizeBytes, cypherBytes, 0, encryptedByteLength);

                var stream = new MemoryStream(cypherBytes);

                using var cs = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
                var buffer = new byte[Math.Min(cypherBytes.Length, 1024)];

                var bytesRead = cs.Read(buffer, 0, buffer.Length);

                while (bytesRead > 0)
                {
                    clearStream.Write(buffer, 0, bytesRead);

                    bytesRead = cs.Read(buffer, 0, buffer.Length);
                }
            }

            return clearStream.ToArray();
        }
    }
}
