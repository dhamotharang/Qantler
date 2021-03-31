using System;

namespace Core.Extensions
{
  public static class StringExtensions
  {
    public static string Mask(this string self, int start = 1, int end = 1)
    {
      if (string.IsNullOrEmpty(self?.Trim()))
      {
        return "";
      }
      return $"{self.Substring(0, start).PadRight(self.Length - end, '*')}{self.Substring(self.Length - end)}";
    }
  }
}
