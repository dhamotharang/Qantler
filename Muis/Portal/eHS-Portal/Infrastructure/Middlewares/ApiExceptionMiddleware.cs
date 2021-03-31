using Core.Http;
using Core.Http.Exceptions;
using eHS.Portal.Infrastructure.Middlewards;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

public static class ApiExceptionMiddlewareExtensions
{
  public static IApplicationBuilder UseApiExceptionMiddleware(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ApiExceptionMiddleware>();
  }
}

namespace eHS.Portal.Infrastructure.Middlewards
{
  public class ApiExceptionMiddleware
  {
    readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionMiddleware> _logger;

    public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
    {
      _next = next;
      _logger = logger;
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
      catch (HttpException e)
      {
        ex = e;
        statusCode = 400;
        errorCode = e.StatusCode;
        message = e.Message;
      }
      catch (NotFoundException e)
      {
        ex = e;
        statusCode = 404;
        errorCode = 404;
        message = e.Message ?? "Not found.";
      }
      catch (ForbiddenException e)
      {
        ex = e;
        statusCode = 403;
        errorCode = 403;
        message = "Access is denied.";
      }
      catch (UnauthorizedException e)
      {
        ex = e;
        statusCode = 401;
        errorCode = 401;
        message = "Unauthorized access.";
      }
      catch (ConnectionException e)
      {
        ex = e;
        statusCode = 503;
        errorCode = 503;
        message = "Unable to connect to server";
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
            _logger.LogError(ex.Message, ex);
          }
        }

        if (statusCode != 200
          && !context.Response.HasStarted)
        {
          context.Response.Clear();
          context.Response.StatusCode = statusCode;
          context.Response.ContentType = "application/json";
          await context.Response.WriteAsync(JsonConvert.SerializeObject(new
          {
            Code = errorCode,
            Message = message
          }));
        }
      }
    }
  }
}
