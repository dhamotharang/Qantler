using System;
using System.Linq;

namespace eHS.Portal.Extensions
{
  public static class StringExtensions
  {
    public static string ToTitleCase(this string self)
    {
      if (string.IsNullOrEmpty(self))
      {
        return self;
      }

      return string.Join(" ", self.Split(" ")
        .Where(e => e.Trim().Length > 0)
        .Select(e =>
        {
          if (e.Length == 1)
          {
            return e.ToUpper();
          }
          return $"{e[0]}".ToUpper() + e.Substring(1).ToLower();
        }));
    }
  }
}
