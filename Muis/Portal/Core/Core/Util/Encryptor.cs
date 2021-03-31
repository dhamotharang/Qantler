using System;
using System.Security.Cryptography;
using System.Text;

namespace Core.Util
{
  public class Encryptor
  {
    private static byte[] _sKeyHash;

    /// <summary>
    /// Encrypt the given string text
    /// </summary>
    /// <param name="text">the text to encryt</param>
    /// <returns>the encrypted string text</returns>
    public static string Encrypt(string secret, string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }
      byte[] byteArray = Encoding.UTF8.GetBytes(text);
      byteArray = Encrypt(secret, byteArray);
      return Convert.ToBase64String(byteArray, 0, byteArray.Length);
    }

    /// <summary>
    /// Decrypt the given previously encrypted text
    /// </summary>
    /// <param name="text">the encrypted text to decrypt</param>
    /// <returns>the decrypted strubg text</returns>
    public static string Decrypt(string secret, string encryptedText)
    {
      if (string.IsNullOrEmpty(encryptedText))
      {
        return encryptedText;
      }
      byte[] encryptedByteArray = Convert.FromBase64String(encryptedText);
      byte[] decryptedByteArray = Decrypt(secret, encryptedByteArray);
      return Encoding.UTF8.GetString(decryptedByteArray);
    }

    /// <summary>
    /// Encrypt given byte array
    /// </summary>
    /// <param name="byteArray">the byte array to encrypt</param>
    /// <returns>encrypted byte array</returns>
    public static byte[] Encrypt(string secret, byte[] byteArray)
    {
      TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
      des.Key = GetKeyHash(secret);
      des.Mode = CipherMode.ECB;
      des.Padding = PaddingMode.PKCS7;

      ICryptoTransform encrypter = des.CreateEncryptor();
      byte[] encryptedByteArray =
          encrypter.TransformFinalBlock(byteArray, 0, byteArray.Length);

      des.Clear();
      return encryptedByteArray;
    }

    /// <summary>
    /// Decrypt given byte array
    /// </summary>
    /// <param name="encrytedByteArray">byte array to decryt</param>
    /// <returns>decrypted byte array</returns>
    public static byte[] Decrypt(string secret, byte[] encrytedByteArray)
    {
      TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
      des.Key = GetKeyHash(secret);
      des.Mode = CipherMode.ECB;
      des.Padding = PaddingMode.PKCS7;

      ICryptoTransform encrypter = des.CreateDecryptor();
      byte[] decryptedByteArray =
          encrypter.TransformFinalBlock(encrytedByteArray, 0, encrytedByteArray.Length);

      des.Clear();
      return decryptedByteArray;
    }

    /// <summary>
    /// Get key hash.
    /// </summary>
    /// <param name="byteArray">the byte array to hash</param>
    /// <returns>the key hash</returns>
    private static byte[] GetKeyHash(string secret)
    {
      if (_sKeyHash == null)
      {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        _sKeyHash = md5.ComputeHash(Encoding.UTF8.GetBytes(secret));
        md5.Clear();
      }
      return _sKeyHash;
    }
  }
}
