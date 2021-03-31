using System;
using Core.Http;

namespace eHS.Portal.Infrastructure.Providers
{
  public interface IHttpRequestProvider
  {
    HttpRequest.Builder BuildUpon(string baseUrl);
  }
}
