using System;
using Core.Http;
using eHS.Portal.Extensions;
using eHS.Portal.Infrastructure.Interceptors;

namespace eHS.Portal.Infrastructure.Providers
{
  public class HttpRequestProvider : IHttpRequestProvider
  {
    readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _contextAccessor;

    public HttpRequestProvider(Microsoft.AspNetCore.Http.IHttpContextAccessor contextAccessor)
    {
      _contextAccessor = contextAccessor;
    }

    public HttpRequest.Builder BuildUpon(string baseUrl)
    {
      var builder = new HttpRequest.Builder().BaseURL(baseUrl)
        .AddInterceptor(new HttpExceptionInterceptor());

      var identity = _contextAccessor.HttpContext.User;
      if (identity != null)
      {
        var token = identity.FindFirst("Token")?.Value;
        if (!string.IsNullOrEmpty(token))
        {
          builder.AddProperty("Authorization", $"Bearer {token}");
        }

        var user = identity.ToOfficer();
        builder.AddParam("userID", user?.ID.ToString());
        builder.AddParam("userName", user?.Name);
        builder.AddParam("userEmail", user?.Email);
      }

      return builder;
    }
  }
}
