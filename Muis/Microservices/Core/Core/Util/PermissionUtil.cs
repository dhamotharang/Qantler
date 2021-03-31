using System;
using System.Text;

namespace Core.Util
{
  public enum Access
  {
    None,
    Active
  }

  public static class PermissionUtil
  {
    public static bool HasPermission(string value, int permission, Access access)
    {
      if (string.IsNullOrEmpty(value)
          || value.Length < permission + 1)
      {
        return false;
      }

      return int.Parse(value[permission].ToString()) == (int)access;
    }

    public static void SetPermission(string value, int permission, Access access, out string result)
    {
      value ??= "";

      StringBuilder sb = new StringBuilder(value);

      int index = permission + 1;
      if (value.Length < index)
      {
        sb.Append('0', index - value.Length);
      }

      sb[permission] = $"{(int)access}"[0];

      result = sb.ToString();
    }

    public static void RemovePermission(string value, int permission, out string result)
    {
      if (string.IsNullOrEmpty(value)
          || value.Length < permission + 1)
      {
        result = value;
        return;
      }

      StringBuilder sb = new StringBuilder(value);
      sb[permission] = '0';
      result = sb.ToString();
    }
  }
}
