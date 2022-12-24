using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class EncryptionHelper
    {
        private const int DEFAULT_KEY_BIT_SIZE = 256;
        private const int DEFAULT_MAC_BIT_SIZE = 128;
        private const int DEFAULT_NONCE_BIT_SIZE = 128;


        #region Public Methods and Operators

        /// <summary>
        /// Simple Decryption & Authentication (AES-GCM) of a UTF8 Message
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="key">The base 64 encoded 256 bit key.</param>
        /// <param name="nonSecretPayloadLength">Length of the optional non-secret payload.</param>
        /// <returns>Decrypted Message</returns>
        public static string DecryptWithKey(string encryptedMessage, string key, int nonSecretPayloadLength = 0)
        {
            if (string.IsNullOrEmpty(encryptedMessage))
            {
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
            }

            var decodedKey = Convert.FromBase64String(key);

            var cipherText = Convert.FromBase64String(encryptedMessage);

            var plaintext = DecryptWithKey(cipherText, decodedKey, nonSecretPayloadLength);

            return Encoding.UTF8.GetString(plaintext);
        }

        /// <summary>
        /// Simple Encryption And Authentication (AES-GCM) of a UTF8 string.
        /// </summary>
        /// <param name="messageToEncrypt">The string to be encrypted.</param>
        /// <param name="key">The base 64 encoded 256 bit key.</param>
        /// <param name="nonSecretPayload">Optional non-secret payload.</param>
        /// <returns>
        /// Encrypted Message
        /// </returns>
        /// <exception cref="System.ArgumentException">Secret Message Required!;secretMessage</exception>
        /// <remarks>
        /// Adds overhead of (Optional-Payload + BlockSize(16) + Message +  HMac-Tag(16)) * 1.33 Base64
        /// </remarks>
        public static string EncryptWithKey(string messageToEncrypt, string key, byte[] nonSecretPayload = null)
        {
            if (string.IsNullOrEmpty(messageToEncrypt))
            {
                throw new ArgumentException("Secret Message Required!", "messageToEncrypt");
            }

            var decodedKey = Convert.FromBase64String(key);

            var plainText = Encoding.UTF8.GetBytes(messageToEncrypt);
            var cipherText = EncryptWithKey(plainText, decodedKey, nonSecretPayload);
            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Helper that generates a random new key on each call.
        /// </summary>
        /// <returns>Base 64 encoded string</returns>
        public static string NewKey()
        {
            var key = new byte[DEFAULT_KEY_BIT_SIZE / 8];
            SecureRandom _random = new SecureRandom();
            _random.NextBytes(key);
            return Convert.ToBase64String(key);
        }

        #endregion

        #region Methods

        public static byte[] DecryptWithKey(byte[] encryptedMessage, byte[] key, int nonSecretPayloadLength = 0)
        {
            //User Error Checks
            CheckKey(key);

            if (encryptedMessage == null || encryptedMessage.Length == 0)
            {
                throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
            }

            using (var cipherStream = new MemoryStream(encryptedMessage))
            using (var cipherReader = new BinaryReader(cipherStream))
            {
                //Grab Payload
                var nonSecretPayload = cipherReader.ReadBytes(nonSecretPayloadLength);

                //Grab Nonce
                var nonce = cipherReader.ReadBytes(DEFAULT_NONCE_BIT_SIZE / 8);

                var cipher = new GcmBlockCipher(new AesEngine());
                var parameters = new AeadParameters(new KeyParameter(key), DEFAULT_MAC_BIT_SIZE, nonce, nonSecretPayload);
                cipher.Init(false, parameters);

                //Decrypt Cipher Text
                var cipherText = cipherReader.ReadBytes(encryptedMessage.Length - nonSecretPayloadLength - nonce.Length);
                var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];

                var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                cipher.DoFinal(plainText, len);

                return plainText;
            }
        }

        public static byte[] EncryptWithKey(byte[] messageToEncrypt, byte[] key, byte[] nonSecretPayload = null)
        {
            //User Error Checks
            CheckKey(key);

            //Non-secret Payload Optional
            nonSecretPayload = nonSecretPayload ?? new byte[] { };

            //Using random nonce large enough not to repeat
            var nonce = new byte[DEFAULT_NONCE_BIT_SIZE / 8];
            SecureRandom _random = new SecureRandom();
            _random.NextBytes(nonce, 0, nonce.Length);

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), DEFAULT_MAC_BIT_SIZE, nonce, nonSecretPayload);
            cipher.Init(true, parameters);

            //Generate Cipher Text With Auth Tag
            var cipherText = new byte[cipher.GetOutputSize(messageToEncrypt.Length)];
            var len = cipher.ProcessBytes(messageToEncrypt, 0, messageToEncrypt.Length, cipherText, 0);
            cipher.DoFinal(cipherText, len);

            //Assemble Message
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    //Prepend Authenticated Payload
                    binaryWriter.Write(nonSecretPayload);
                    //Prepend Nonce
                    binaryWriter.Write(nonce);
                    //Write Cipher Text
                    binaryWriter.Write(cipherText);
                }
                return combinedStream.ToArray();
            }
        }

        private static void CheckKey(byte[] key)
        {
            if (key == null || key.Length != DEFAULT_KEY_BIT_SIZE / 8)
            {
                throw new ArgumentException(String.Format("Key needs to be {0} bit! actual:{1}", DEFAULT_KEY_BIT_SIZE, key?.Length * 8), "key");
            }
        }

        public static byte[] DecryptFileWithKey(byte[] encryptedMemoryStream, Guid key, int nonSecretPayloadLength = 0)
        {
            var decodedKey = Convert.FromBase64String(key.ToString().Replace("-", ""));
            PadToMultipleOf(ref decodedKey, DEFAULT_KEY_BIT_SIZE / 8);
            return DecryptWithKey(encryptedMemoryStream, decodedKey, nonSecretPayloadLength);
        }

        public static byte[] EncryptFileWithKey(byte[] plainMemoryStream, Guid key, byte[] nonSecretPayload = null)
        {
            var decodedKey = Convert.FromBase64String(key.ToString().Replace("-", ""));
            PadToMultipleOf(ref decodedKey, DEFAULT_KEY_BIT_SIZE / 8);
            return EncryptWithKey(plainMemoryStream, decodedKey, nonSecretPayload);
        }


        private static void PadToMultipleOf(ref byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len);
        }
        #endregion
    }
}
