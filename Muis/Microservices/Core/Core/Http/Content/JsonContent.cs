using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Core.Http
{
  public class JsonContent : Content
  {
    object _data;

    public JsonContent(object data)
    {
      _data = data;
    }

    internal override void WriteContent(HttpRequestMessage request)
    {
      if (_data != null)
      {
        request.Content = new System.Net.Http.StringContent(
          JsonConvert.SerializeObject(_data),
          Encoding.UTF8,
          "application/json");
      }
    }
  }
}
