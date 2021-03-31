using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class NotesSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public NotesSdk(string url, IHttpRequestProvider requestProvider)
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

    public Task<IList<Notes>> GetNotes(long requestID)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/notes")
        .AddParam("requestID", $"{requestID}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Notes>>();
    }
  }
}
