using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Core.Util
{
  public class Gzip
  {
    public static string Compress(string text)
    {
      if (string.IsNullOrEmpty(text?.Trim()))
      {
        return text;
      }

      return Compress(Encoding.UTF8.GetBytes(text));
    }

    public static string Compress(byte[] textInByte)
    {
      string result = null;

      using (var ms = new MemoryStream())
      {
        using (var gzip = new GZipStream(ms, CompressionMode.Compress))
        {
          gzip.Write(textInByte, 0, textInByte.Length);
        }

        result = Convert.ToBase64String(ms.ToArray());
      }
      return result;
    }

    public static string Decompress(string base64String)
    {
      if (string.IsNullOrEmpty(base64String?.Trim()))
      {
        return base64String;
      }

      return Decompress(Convert.FromBase64String(base64String));
    }

    public static string Decompress(byte[] compressedInByte)
    {
      if (compressedInByte == null)
      {
        return null;
      }

      string result = null;
      using (var ms = new MemoryStream(compressedInByte))
      {
        using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
        {
          using (var sr = new StreamReader(gzip))
          {
            result = sr.ReadToEnd();
          }
        }
      }
      return result;
    }
  }
}
