using System;
using System.Security.Claims;
using Core.Util;
using eHS.Portal.Model;

namespace eHS.Portal.Extensions
{
  public static class ClaimsPrincipalExtensions
  {
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentNullException(nameof(principal));
      }

      var userID = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      return string.IsNullOrEmpty(userID) ? Guid.Empty : new Guid(userID);
    }

    public static string GetName(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentNullException(nameof(principal));
      }

      return principal.Identity.Name;
    }

    public static string GetPermissions(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentNullException(nameof(principal));
      }

      return principal.FindFirst("Permissions")?.Value;
    }

    public static Officer ToOfficer(this ClaimsPrincipal principal)
    {
      return new Officer
      {
        ID = principal.GetUserId(),
        Name = principal.GetName(),
        Email = principal.FindFirstValue(ClaimTypes.Email)
      };
    }

    public static string GetToken(this ClaimsPrincipal principal)
    {
      if (principal == null)
      {
        throw new ArgumentNullException(nameof(principal));
      }

      return principal.FindFirst("Token")?.Value;
    }

    public static bool HasPermission(this ClaimsPrincipal principal)
    {
      var hasPermission = false;
      foreach(Permission permission in Enum.GetValues(typeof(Permission)))
      {
        hasPermission = principal.HasPermission(permission);
        if (hasPermission)
        {
          break;
        }
      }
      return hasPermission;
    }

    public static bool HasPermission(this ClaimsPrincipal principal, params Permission[] permissions)
    {
      var p = principal.GetPermissions();
      foreach (var permission in permissions)
      {
        if (PermissionUtil.HasPermission(p, (int)permission, Access.Active))
        {
          return true;
        }
      }
      return false;
    }
  }
}
