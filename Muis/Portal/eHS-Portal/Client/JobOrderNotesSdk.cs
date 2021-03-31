using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class JobOrderNotesSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public JobOrderNotesSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<Notes> AddNotes(Notes notes)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/notes")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(notes))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Notes>();
    }

    public Task<IList<Notes>> GetNotes(long jobOrderID)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/notes")
        .AddParam("jobOrderID", $"{jobOrderID}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Notes>>();
    }
  }
}
