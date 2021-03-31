using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Http
{
  public class JsonDeserializerInterceptor : AbsInterceptor
  {
    public override Task<T> PostHandle<T>(HttpResponseMessage response)
    {
      if (!response.IsSuccessStatusCode)
      {
        return base.PostHandle<T>(response);
      }

      T result = default(T);

      var data = response.Content.ReadAsStringAsync().Result;
      if (   string.IsNullOrEmpty(data?.Trim())
          || data == "null")
      {
        result = default(T);
      }
      else if (typeof(T) == typeof(string))
      {
        result = (T)(object)data;
      }
      else
      {
        result = JsonConvert.DeserializeObject<T>(data);
      }
      return Task.FromResult(result);
    }
  }
}
