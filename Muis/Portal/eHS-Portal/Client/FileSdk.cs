using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using Core.Http.Interceptors;
using eHS.Portal.Infrastructure.Providers;

namespace eHS.Portal.Client
{
  public class FileSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public FileSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<byte[]> Download(Guid guid)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/file/{guid}")
        .AddInterceptor(new ResponseAsByteInterceptor())
        .Execute<byte[]>();
    }

    public Task<IList<Model.File>> Upload(FileContent filecontent)
    {
      var options = new Core.Http.MultipartFormDataContent();
      options.Add("file", filecontent);

      return _requestProvider.BuildUpon(_url)
        .Uri("/api/file")
        .Method(HttpMethod.Post)
        .Content(options)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Model.File>>();
    }

    public Task<string> Delete(Guid guid)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/file/{guid}")
        .Method(HttpMethod.Delete)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }
  }
}
