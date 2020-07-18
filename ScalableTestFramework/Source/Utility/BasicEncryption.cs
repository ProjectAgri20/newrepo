using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Provides a simple encryption method for obfuscating strings.
    /// </summary>
    public static class BasicEncryption
    {
        private static readonly byte[] _salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// Encrypts the specified clear text using the specified password.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="password">The password.</param>
        /// <returns>An encrypted string.</returns>
        public static string Encrypt(string clearText, string password)
        {
            if (string.IsNullOrEmpty(clearText))
            {
                return string.Empty;
            }

            // Convert the input string into a byte array
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            // Transform the data using CreateEncryptor
            byte[] encryptedData = CryptoTransform(clearBytes, password, n => n.CreateEncryptor());

            // Convert the resulting byte array into a Base64 encoded string
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the specified cipher text using the specified password.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="password">The password.</param>
        /// <returns>The decrypted string.</returns>
        public static string Decrypt(string cipherText, string password)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return string.Empty;
            }

            // Convert the encrypted string back into a byte array
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Transform the data using CreateDecryptor
            byte[] decryptedData = CryptoTransform(cipherBytes, password, n => n.CreateDecryptor());

            // Convert the resulting byte array back into a string
            return Encoding.Unicode.GetString(decryptedData);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static byte[] CryptoTransform(byte[] data, string password, Func<SymmetricAlgorithm, ICryptoTransform> createTransform)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                // Create the key and initialization vector using the password.
                using (PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, _salt))
                {
                    // Divide by 8 to account for bit/byte conversion
                    rijndael.Key = pdb.GetBytes(rijndael.KeySize / 8);
                    rijndael.IV = pdb.GetBytes(rijndael.BlockSize / 8);
                }

                // Create a CryptoStream to perform the encryption/decryption
                // and a MemoryStream to accept the resulting bytes.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    ICryptoTransform transform = createTransform(rijndael);
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                    {
                        // Perform the encryption and return the result
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.Close();
                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}
