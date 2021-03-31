using Core.Http;
using eHS.Portal.DTO;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using eHS.Portal.Models.JobOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class JobOrderSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public JobOrderSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<JobOrder>> Query(JobOrderOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/joborder/query")
        .AddInterceptor(new JsonDeserializerInterceptor());
      if (options.ID > 0L)
      {
        builder.AddParam("id", $"{options.ID}");
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

      if (options.AssignedTo?.Any() ?? false)
      {
        for (int i = 0; i < options.AssignedTo.Count; i++)
        {
          builder.AddParam($"assignedTo[{i}]", options.AssignedTo[i].ToString());
        }
      }
      builder.AddParam("from", options.From?.UtcDateTime.ToString());
      builder.AddParam("to", options.To?.UtcDateTime.ToString());

      return builder.Execute<IList<JobOrder>>();
    }

    public Task<JobOrder> GetByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/joborder/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<JobOrder>();
    }

    public Task<Model.Officer> GetInvities(long id)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/joborder/{id}/invities")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<Model.Officer>();
    }

    public Task<string> AddInvitee(long jobID, Officer officer)
    {
      return _requestProvider.BuildUpon(_url)
       .Uri($"/api/joborder/{jobID}/invitee")
       .Method(HttpMethod.Post)
       .Content(new JsonContent(officer))
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<string>();
    }

    public Task<string> DeleteInvitee(long jobID, Guid officerID)
    {
      return _requestProvider.BuildUpon(_url)
       .BaseURL(_url)
       .Uri($"/api/joborder/{jobID}/invitee/{officerID}")
       .Method(HttpMethod.Delete)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<string>();
    }

    public async Task<ScheduleJobOrderParam> Submit(ScheduleJobOrderParam jobOrder)
    {
      return await _requestProvider.BuildUpon(_url)
        .BaseURL(_url)
        .Uri($"/api/joborder")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(jobOrder))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<ScheduleJobOrderParam>();
    }

    public async Task<string> Reschedule(long id, RescheduleParam param)
    {
      return await _requestProvider.BuildUpon(_url)
       .BaseURL(_url)
       .Uri($"/api/joborder/{id}/reschedule")
       .Method(HttpMethod.Put)
       .Content(new JsonContent(param))
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<string>();
    }

    public async Task<string> Schedule(long id, ScheduleParam param)
    {
      return await _requestProvider.BuildUpon(_url)
       .BaseURL(_url)
       .Uri($"/api/joborder/{id}/schedule")
       .Method(HttpMethod.Put)
       .Content(new JsonContent(param))
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<string>();
    }

    public async Task<string> Cancel(long id, CancelParam param)
    {
      return await _requestProvider.BuildUpon(_url)
       .BaseURL(_url)
       .Uri($"/api/joborder/{id}/cancel")
       .Method(HttpMethod.Post)
       .Content(new JsonContent(param))
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<string>();
    }
  }
}