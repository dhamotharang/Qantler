using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using Finance.API.Middleware;
using Core.API.Exceptions;

public static class ApiExceptionMiddlewareExtensions
{
  public static IApplicationBuilder UseApiExceptionMiddleware(
    this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ApiExceptionMiddleware>();
  }
}

namespace Finance.API.Middleware
{
  public class ApiExceptionMiddleware
  {
    readonly RequestDelegate _next;

    private static Logger _logger = LogManager.GetCurrentClassLogger();

    public ApiExceptionMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      int statusCode = 200;
      int errorCode = 0;
      string message = null;
      Exception ex = null;

      try
      {
        await _next(context);
      }
      catch (BadRequestException e)
      {
        ex = e;
        statusCode = 400;
        errorCode = e.Code;
        message = e.Message;
      }
      catch (AuthenticationException e)
      {
        ex = e;
        statusCode = e.Code;
        errorCode = e.Code;
        message = e.Message ?? "Authentication failed.";
      }
      catch (UnauthorizedException e)
      {
        ex = e;
        statusCode = 401;
        errorCode = e.Code;
        message = "Aunauthorized access.";
      }
      catch (Exception e)
      {
        ex = e;
        statusCode = 500;
        errorCode = 500;
        message = "Internal server error";
      }
      finally
      {
        if (ex != null)
        {
          Console.WriteLine("Exception: {0}", ex.StackTrace);
          if (statusCode == 500)
          {
            _logger.Error(ex);
          }
        }

        if (statusCode != 200
            && !context.Response.HasStarted)
        {
          context.Response.Clear();
          context.Response.StatusCode = statusCode;
          context.Response.ContentType = "application/json";
          await context.Response.WriteAsync(
            JsonConvert.SerializeObject(new
            {
              Code = errorCode,
              Message = message
            }));
        }
      }
    }
  }
}