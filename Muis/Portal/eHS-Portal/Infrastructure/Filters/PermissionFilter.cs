using System;
using Core.Http.Exceptions;
using Core.Util;
using eHS.Portal.Extensions;
using eHS.Portal.Model;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eHS.Portal.Infrastructure.Filters
{
  public class PermissionFilter : ActionFilterAttribute,
                                  IOrderedFilter
  {
    readonly Permission[] _permissions;

    public PermissionFilter(params Permission[] permissions)
    {
      Order = 200;
      _permissions = permissions;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
      base.OnActionExecuting(context);

      var p = context.HttpContext.User.GetPermissions();

      bool hasPermission = false;

      foreach(var permission in _permissions)
      {
        if (PermissionUtil.HasPermission(p, (int)permission, Access.Active))
        {
          hasPermission = true;
          break;
        }
      }

      if (!hasPermission)
      {
        throw new ForbiddenException(string.Join(", ", _permissions));
      }
    }
  }
}
