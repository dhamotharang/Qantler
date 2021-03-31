using System;
using System.Net.Http;
using System.Text;

namespace Core.Http
{
  public class StringContent : Content
  {
    string _value;

    public StringContent(string value)
    {
      _value = value;
    }

    internal override void WriteContent(HttpRequestMessage request)
    {
      request.Content = new System.Net.Http.StringContent(_value, Encoding.UTF8, "text/plain");
    }
  }
}
