using System;
using System.Net.Sockets;
using Core.Http;
using Core.Http.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace eHS.Portal.Infrastructure.Filters
{
  public class ExceptionFilter : IExceptionFilter
  {
    public ExceptionFilter()
    {
    }

    public void OnException(ExceptionContext context)
    {
      if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
      {
        return;
      }

      var page = context.Exception is UnauthorizedException ? "401"
        : context.Exception is ForbiddenException ? "403"
        : context.Exception is NotFoundException ? "404"
        : context.Exception is HttpException ? "400"
        : context.Exception is ConnectionException ? "503"
        : "500";

      context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
      {
        area = "",
        controller = "Error",
        action = "index",
        code = page,
        message = context.Exception.Message ?? ""
      }));
      ;
    }
  }
}
