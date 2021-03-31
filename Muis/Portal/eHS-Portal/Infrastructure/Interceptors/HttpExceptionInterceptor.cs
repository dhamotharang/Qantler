using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using Core.Http.Exceptions;
using Newtonsoft.Json;

namespace eHS.Portal.Infrastructure.Interceptors
{
  public class HttpExceptionInterceptor : AbsInterceptor
  {
    public override Task<T> PostHandle<T>(HttpResponseMessage response)
    {
      if (response.IsSuccessStatusCode)
      {
        return null;
      }

      if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        return Task.Run<T>(async () =>
        {
          var errorBody = await response.Content.ReadAsStringAsync();

          var data = JsonConvert.DeserializeObject<ErrorData>(errorBody);

          throw new HttpException(data.Code, data.Message);
        });
      }
      else if (response.StatusCode == HttpStatusCode.Forbidden)
      {
        throw new ForbiddenException();
      }
      else if (response.StatusCode == HttpStatusCode.Unauthorized)
      {
        throw new UnauthorizedException();
      }
      else if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new NotFoundException();
      }
      else if ((int)response.StatusCode > 500)
      {
        throw new ConnectionException();
      }

      throw new Exception();
    }
  }

  class ErrorData
  {
    public int Code { get; set; }
    public string Message { get; set; }
  }
}
