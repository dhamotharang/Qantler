using System;
using System.Text;

namespace Identity.API.Util
{
  public static class RandomStringGenerator
  {
    private static readonly int DefaultLength = 6;
    private static readonly string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789=-~!@#$%^&*()";

    /// <summary>
    /// Generates a random otp code with default length of 6.
    /// </summary>
    /// <returns></returns>
    public static string Generate()
    {
      return Generate(DefaultLength);
    }

    /// <summary>
    /// Generates random otp code with specified length.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Generate(int length)
    {
      StringBuilder sb = new StringBuilder();

      Random random = new Random();
      for (int i = 0; i < length; i++)
      {
        int cIndex = (int)(random.NextDouble() * Characters.Length);
        sb.Append(Characters[cIndex]);
      }
      return sb.ToString();
    }
  }
}
