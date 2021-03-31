using System;

namespace Core.API
{
  public static class StringUtils
  {
    public static string NullIfEmptyOrNull(string val)
    {
      return string.IsNullOrEmpty(val?.Trim()) ? null : val.Trim();
    }
  }
}
