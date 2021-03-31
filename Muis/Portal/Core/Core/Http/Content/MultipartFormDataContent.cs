using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Core.Http
{
  public class MultipartFormDataContent : Content
  {
    readonly IDictionary<string, object> _contents = new Dictionary<string, object>();

    internal override void WriteContent(HttpRequestMessage request)
    {
      var content = new System.Net.Http.MultipartFormDataContent();

      foreach (var item in _contents)
      {
        if (item.Value is string s)
        {
          content.Add(new System.Net.Http.StringContent(s, Encoding.UTF8), item.Key);
        }
        else if (item.Value is FileContent file)
        {
          content.Add(new StreamContent(file.Steam), item.Key, file.FileName ?? item.Key);
        }
      }

      request.Content = content;
    }

    public void Add(string name, string value)
    {
      _contents[name] = value;
    }

    public void Add(string name, FileContent file)
    {
      _contents[name] = file;
    }
  }

  public class FileContent
  {
    public Stream Steam { get; set; }
    public string FileName { get; set; }
  }
}
