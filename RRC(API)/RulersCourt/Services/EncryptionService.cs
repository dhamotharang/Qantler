using System;
using System.IO;
using System.Security.Cryptography;

namespace RulersCourt.Services
{
    public class EncryptionService
    {
        private readonly KeyInfo _keyInfo;

        public EncryptionService(KeyInfo keyInfo = null)
        {
            _keyInfo = keyInfo;
        }

        public Stream Encrypt(Stream input)
        {
            return EncryptFileToStream_Aes(input, _keyInfo.Key, _keyInfo.Iv);
        }

        public Stream Decrypt(Stream cipherInput)
        {
            return DecryptFileFromStream_Aes(cipherInput, _keyInfo.Key, _keyInfo.Iv);
        }

        public string Decrypt_AES(string cipherText)
        {
            return DecryptStringFromBytes_Aes(cipherText, _keyInfo.Key, _keyInfo.Iv);
        }

        private static Stream EncryptFileToStream_Aes(Stream fileInput, byte[] key, byte[] iv)
        {
            var fsEncrypted = new MemoryStream();

            if (fileInput == null || fileInput.Length <= 0)
                throw new ArgumentNullException(nameof(fileInput));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var cryptostream = new CryptoStream(fsEncrypted, encryptor, CryptoStreamMode.Write);

            var bytearrayinput = new byte[fileInput.Length];
            fileInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            fileInput.Close();

            fsEncrypted.Flush();
            fsEncrypted.Position = 0;
            return fsEncrypted;
        }

        private static Stream DecryptFileFromStream_Aes(Stream cipherInput, byte[] key, byte[] iv)
        {
            var sOutputFilename = new MemoryStream();
            if (cipherInput == null || cipherInput.Length <= 0)
                throw new ArgumentNullException(nameof(cipherInput));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Padding = PaddingMode.Zeros;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cryptostreamDecr = new CryptoStream(cipherInput, decryptor, CryptoStreamMode.Read);

            var buffer = new byte[1024];
            var read = cryptostreamDecr.Read(buffer, 0, buffer.Length);
            while (read > 0)
            {
                sOutputFilename.Write(buffer, 0, read);
                read = cryptostreamDecr.Read(buffer, 0, buffer.Length);
            }

            cryptostreamDecr.Close();
            return sOutputFilename;
        }

        public static string EncryptStringToBytes_Aes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));
            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptStringFromBytes_Aes(string cipherTextStr, byte[] key, byte[] iv)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextStr);
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            string plaintext;
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }
}
