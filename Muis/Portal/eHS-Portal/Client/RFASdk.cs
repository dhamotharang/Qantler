using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class RFASdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public RFASdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<RFA> Submit(RFA data)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/rfa")
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(data))
        .Execute<RFA>();
    }

    public Task<string> UpdateRFAStatus(long id, RFAStatus status)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/rfa/{id}/status")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .AddParam("status", status.ToString())
        .Execute<string>();
    }

    public Task<RFA> GetRFAByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/rfa/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<RFA>();
    }

    public Task<RFA> ExtendRFADueDate(long id,
      string notes,
      DateTimeOffset toDate)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/rfa/{id}/extend")
        .Method(HttpMethod.Post)
        .AddParam("notes", string.IsNullOrEmpty(notes?.Trim()) ? null : notes.Trim())
        .AddParam("toDate", toDate.UtcDateTime.ToString())
        .Execute<RFA>();
    }

    public Task<string> Delete(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/rfa/{id}")
        .Method(HttpMethod.Delete)
        .Execute<string>();
    }

    public Task<IList<RFA>> List(RFAOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/rfa/list")
        .AddParam("id", options.ID.HasValue ? $"{options.ID.Value}" : null)
        .AddParam("requestID", options.RequestID.HasValue ? $"{options.RequestID.Value}" : null)
        .AddParam("createdOn", options.CreatedOn?.UtcDateTime.ToString())
        .AddParam("dueOn", options.DueOn?.UtcDateTime.ToString())
        .AddParam("raisedBy", options.RaisedBy?.ToString())
        .AddParam("customer", options.Customer)
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (options.Status?.Any() ?? false)
      {
        for (int i = 0; i < options.Status.Count; i++)
        {
          builder.AddParam($"status[{i}]", $"{(int)options.Status[i]}");
        }
      }

      return builder.Execute<IList<RFA>>();
    }
  }
}
