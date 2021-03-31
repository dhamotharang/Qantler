using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Http.Interceptors
{
  public class ResponseAsByteInterceptor : AbsInterceptor
  {
    public override async Task<T> PostHandle<T>(HttpResponseMessage response)
    {
      if (!response.IsSuccessStatusCode)
      {
        return await base.PostHandle<T>(response);
      }
      var result = await response.Content.ReadAsByteArrayAsync();
      return (T)Convert.ChangeType(result, typeof(T));
    }
  }
}
