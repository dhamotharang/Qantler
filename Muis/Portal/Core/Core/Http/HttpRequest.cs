using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Http
{
  public class HttpRequest
  {
    string BaseURL { get; set; }
    string Uri { get; set; }
    Content Content { get; set; }
    HttpMethod Method { get; set; } = HttpMethod.Get;
    IList<IInterceptor> Interceptors { get; } = new List<IInterceptor>();
    int MaxRetries { get; set; } = 1;

    readonly IDictionary<string, string> Properties = new Dictionary<string, string>();
    readonly IDictionary<string, string> RequestParams = new Dictionary<string, string>();

    public async Task<T> Execute<T>()
    {
      var result = default(T);
      try
      {
        int retry = 0;

        while (retry < MaxRetries)
        {
          try
          {
            var request = BuildRequest(BuildUri());
            PreHandle(request);
            var response = await ExecuteRequest(request);
            result = await PostHandle<T>(response);
            break;
          }
          catch (Exception e)
          {
            retry++;

            if (retry >= MaxRetries)
            {
              throw e;
            }

            Thread.Sleep(300);
          }
        }
      }
      catch (Exception ex)
      {
        Catch(ex);
      }

      OnCompleted(result);
      return result;
    }

    UriBuilder BuildUri()
    {
      var builder = new UriBuilder($"{BaseURL}{Uri}");

      // Append request query parameters
      if (RequestParams.Count > 0)
      {
        StringBuilder query = new StringBuilder();

        foreach (var param in RequestParams)
        {
          if (string.IsNullOrEmpty(param.Value?.Trim()))
          {
            continue;
          }

          if (query.Length > 0)
          {
            query.Append("&");
          }
          query.Append($"{param.Key}={WebUtility.UrlEncode(param.Value)}");
        }
        builder.Query = query.ToString();
      }

      return builder;
    }

    HttpRequestMessage BuildRequest(UriBuilder uri)
    {
      var request = new HttpRequestMessage(Method, new Uri(uri.ToString()));
      request.Headers.CacheControl = CacheControlHeaderValue.Parse("no-cache, no-store, must-revalidate");

      if (Properties.Count > 0)
      {
        foreach (var prop in Properties)
        {
          request.Headers.Add(prop.Key, prop.Value);
        }
      }

      Content?.WriteContent(request);

      return request;
    }

    void PreHandle(HttpRequestMessage request)
    {
      foreach (var i in Interceptors)
      {
        i.PreHandle(request);
      }
    }

    async Task<HttpResponseMessage> ExecuteRequest(HttpRequestMessage request)
    {
      return await InternalExecuteRequest(request);
    }

    async Task<HttpResponseMessage> InternalExecuteRequest(HttpRequestMessage request)
    {
      var handler = new HttpClientHandler();

#if DEBUG
      handler.ServerCertificateCustomValidationCallback += (s, c, ch, e) =>
      {
        return true;
      };
#endif

      var client = new HttpClient(handler)
      {
        Timeout = TimeSpan.FromMinutes(1.5),
        DefaultRequestHeaders =
        {
          CacheControl = CacheControlHeaderValue.Parse("no-cache, no-store, must-revalidate"),
          Pragma = { NameValueHeaderValue.Parse("no-cache") }
        }
      };

      return await client.SendAsync(request);
    }

    async Task<T> PostHandle<T>(HttpResponseMessage response)
    {
      var result = default(T);

      foreach (var i in Interceptors)
      {
        var handler = i.PostHandle<T>(response);
        if (handler != null)
        {
          result = await handler;
          break;
        }
      }

      return result;
    }

    void Catch(Exception ex)
    {
      foreach (var i in Interceptors)
      {
        if (!i.OnError(ex))
        {
        }
      }

      if (ex is SocketException
        || ex.InnerException is SocketException)
      {
        throw new ConnectionException(ex.Message);
      }

      throw ex;
    }

    void OnCompleted<T>(T data)
    {
      foreach (var i in Interceptors)
      {
        i.OnCompleted(data);
      }
    }

    public class Builder
    {
      readonly HttpRequest _request = new HttpRequest();

      public Builder BaseURL(string baseUrl)
      {
        _request.BaseURL = baseUrl;
        return this;
      }

      public Builder Uri(string uri)
      {
        _request.Uri = uri;
        return this;
      }

      public Builder Method(HttpMethod method)
      {
        _request.Method = method;
        return this;
      }

      public Builder AddProperty(string key, string value)
      {
        _request.Properties.Add(key, value);
        return this;
      }

      public Builder AddProperties(IDictionary<string, string> param)
      {
        foreach (var item in param)
        {
          _request.Properties.Add(item.Key, item.Value);
        }
        return this;
      }

      public Builder AddParam(string key, string value)
      {
        _request.RequestParams.Add(key, value);
        return this;
      }

      public Builder AddParams(IDictionary<string, string> param)
      {
        foreach (var item in param)
        {
          _request.RequestParams.Add(item.Key, item.Value);
        }
        return this;
      }

      public Builder AddInterceptor(IInterceptor interceptor)
      {
        _request.Interceptors.Add(interceptor);
        return this;
      }

      public Builder AddInterceptors(IEnumerable<IInterceptor> interceptors)
      {
        foreach (var interceptor in interceptors)
        {
          _request.Interceptors.Add(interceptor);
        }
        return this;
      }

      public Builder Content(Content content)
      {
        _request.Content = content;
        return this;
      }

      public Builder MaxRetries(int maxRetries)
      {
        _request.MaxRetries = maxRetries;
        return this;
      }

      public HttpRequest Build()
      {
        return _request;
      }

      public Task<T> Execute<T>()
      {
        return _request.Execute<T>();
      }
    }
  }
}
