using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.DTO;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class RequestSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public RequestSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<Request>> Query(RequestOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/request/query")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (options.ID > 0L)
      {
        builder.AddParam("id", $"{options.ID}");
      }

      if (options.CustomerID.HasValue)
      {
        builder.AddParam("customerId", $"{options.CustomerID}");
      }

      if (!string.IsNullOrEmpty(options.RefID?.Trim()))
      {
        builder.AddParam("refID", options.RefID.Trim());
      }

      if (!string.IsNullOrEmpty(options.CustomerCode?.Trim()))
      {
        builder.AddParam("customerCode", options.CustomerCode.Trim());
      }

      if (!string.IsNullOrEmpty(options.Customer?.Trim()))
      {
        builder.AddParam("customer", options.Customer.Trim());
      }

      if (!string.IsNullOrEmpty(options.Premise?.Trim()))
      {
        builder.AddParam("premise", options.Premise.Trim());
      }

      if (options.Status?.Any() ?? false)
      {
        for (int i = 0; i < options.Status.Count; i++)
        {
          builder.AddParam($"status[{i}]", $"{(int)options.Status[i]}");
        }
      }

      if (options.Type?.Any() ?? false)
      {
        for (int i = 0; i < options.Type.Count; i++)
        {
          builder.AddParam($"type[{i}]", $"{(int)options.Type[i]}");
        }
      }

      if (options.RFAStatus != null)
      {
        builder.AddParam($"rfaStatus", $"{(int)options.RFAStatus}");
      }

      if (options.AssignedTo?.Any() ?? false)
      {
        for (int i = 0; i < options.AssignedTo.Count; i++)
        {
          builder.AddParam($"assignedTo[{i}]", options.AssignedTo[i].ToString());
        }
      }

      if (options.EscalateStatus != null)
      {
        builder.AddParam($"escalateStatus", $"{(int)options.EscalateStatus}");
      }

      if (options.StatusMinor?.Any() ?? false)
      {
        for (int i = 0; i < options.StatusMinor.Count; i++)
        {
          builder.AddParam($"statusMinor[{i}]", $"{(int)options.StatusMinor[i]}");
        }
      }

      builder.AddParam("from", options.From?.UtcDateTime.ToString())
        .AddParam("to", options.To?.UtcDateTime.ToString());

      return builder.Execute<IList<Request>>();
    }

    public Task<Request> GetByID(long id)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<Request>();
    }

    public Task<Request> GetByIDBasic(long id)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/basic")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<Request>();
    }

    public Task<IList<Request>> GetRelatedRequest(long id)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/related")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<IList<Request>>();
    }
    public Task<Request> Submit(Request data)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/request")
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(data))
        .Execute<Request>();
    }

    public Task<Model.Review> SubmitReview(Review data)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{data.RequestID}/review")
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(data))
        .Execute<Model.Review>();
    }

    public Task<string> EscalateRequest(long requestID, EscalateStatus status, string remarks)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{requestID}/escalate")
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .AddParam("remarks", remarks)
        .AddParam("status", $"{(int)status}")
        .Execute<string>();
    }

    public async Task<string> KIV(long id, string notes, DateTimeOffset remindOn)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/kiv")
        .Method(HttpMethod.Post)
        .AddParam("notes", notes)
        .AddParam("remindOn", remindOn.UtcDateTime.ToString())
        .Execute<string>();
    }

    public async Task<string> RevertKIV(long id)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/kiv")
        .Method(HttpMethod.Delete)
        .Execute<string>();
    }

    public async Task<string> ScheduledInspection(long id, long jobID, string notes,
      DateTimeOffset scheduledOn, DateTimeOffset scheduledOnTo)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/inspection")
        .Method(HttpMethod.Post)
        .AddParam("jobID", $"{jobID}")
        .AddParam("scheduledOn", scheduledOn.UtcDateTime.ToString())
        .AddParam("scheduledOnTo", scheduledOnTo.UtcDateTime.ToString())
        .AddParam("notes", notes)
        .Execute<string>();
    }

    public Task<string> Recommend(RecommendParam recommendParam)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/recommend")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(recommendParam))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<string> Review(RecommendParam recommendParam)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/review")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(recommendParam))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<string> Approve(IList<Review> data)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/approve")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(data))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<string> Reaudit(long requestID, string remarks)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{requestID}/reaudit")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(new
        {
          notes = remarks
        }))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<IList<Review>> GetReviews(long[] requestIDs)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/reviews")
        .Method(HttpMethod.Get)
        .AddInterceptor(new JsonDeserializerInterceptor());

      for (int i = 0; i < requestIDs.Length; i++)
      {
        long id = requestIDs[i];
        builder.AddParam($"requestIDs[{i}]", $"{id}");
      }

      return builder.Execute<IList<Review>>();
    }

    public async Task Reassign(long requestID, Officer toOfficer, string notes)
    {
      await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{requestID}/reassign")
        .Method(HttpMethod.Post)
        .AddParam("officerID", toOfficer.ID.ToString())
        .AddParam("officerName", toOfficer.Name)
        .AddParam("notes", notes)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public async Task UpdateRequestStatus(long id, RequestStatus status,
      RequestStatusMinor? statusMinor)
    {
      await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/status")
        .Method(HttpMethod.Put)
        .AddParam("status", $"{(int)status}")
        .AddParam("statusMinor", statusMinor != null ? $"{(int)statusMinor}" : "")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public async Task ProceedForReview(ProceedForReviewParam param)
    {
      await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{param.RequestID}/proceedForReview")
        .Method(HttpMethod.Put)
        .AddParam("officerID", param.AssignOfficer?.ID.ToString())
        .AddParam("officerName",
        param.AssignOfficer != null ? param.AssignOfficer.Name : string.Empty)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public async Task Reinstate(long id, ReinstateParam param)
    {
      await _requestProvider.BuildUpon(_url)
        .Uri($"/api/request/{id}/reinstate")
        .Method(HttpMethod.Put)
        .AddParam("notes", param.Notes)
        .AddParam("refID", param.Reason.ID.ToString())
        .AddParam("remarks", param.Reason.Value)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

  }
}