using Core.Http;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using eHS.Portal.Infrastructure.Providers;
using System.Collections.Generic;
using eHS.Portal.Model;
using System.Linq;

namespace eHS.Portal.Client
{
  public class CustomerSdk
  {
    readonly string _url;
    readonly string _requestUrl;
    readonly IHttpRequestProvider _requestProvider;

    public CustomerSdk(string url, string requestUrl, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestUrl = requestUrl;
      _requestProvider = requestProvider;
    }

    public Task<Customer> GetCustomerContactInfo(string name, string altID)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/customer/find")
        .AddParam("name", name)
        .AddParam("altID", altID)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }

    public async Task<Customer> Submit(Customer data)
    {
      return await _requestProvider.BuildUpon(_requestUrl)
       .Uri("/api/customer")
       .Method(HttpMethod.Post)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Content(new JsonContent(data))
       .Execute<Customer>();
    }

    public Task<Customer> GetByID(Guid id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/customer/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }

    public Task<Customer> GetRequestCustomerByID(Guid id)
    {
      return _requestProvider.BuildUpon(_requestUrl)
        .Uri($"/api/customer/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }

    public Task<Customer> SetCodeID(Guid id, long? codeID)
    {
      return _requestProvider.BuildUpon(_requestUrl)
        .Uri($"/api/customer/{id}/code")
        .Method(HttpMethod.Put)
        .AddParam("codeID", codeID != null ? $"{codeID.Value}" : null)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }

    public Task<Customer> SetGroupCodeID(Guid id, long? codeID)
    {
      return _requestProvider.BuildUpon(_requestUrl)
        .Uri($"/api/customer/{id}/groupCode")
        .Method(HttpMethod.Put)
        .AddParam("codeID", codeID != null ? $"{codeID.Value}" : null)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }

    public Task<Customer> SetOfficerInCharge(Guid id, Guid? officerID = null)
    {
      return _requestProvider.BuildUpon(_requestUrl)
        .Uri($"/api/customer/{id}/officerInCharge")
        .Method(HttpMethod.Put)
        .AddParam("officerID", officerID != null ? $"{officerID.Value}" : null)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }

    public Task<IList<Certificate360>> Query(CustomerOptions options)
    {
      var builder = _requestProvider.BuildUpon(_requestUrl)
        .Uri("/api/customer/query")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (options.ID.HasValue)
      {
        builder.AddParam("ID", $"{options.ID}");
      }

      if (!string.IsNullOrEmpty(options.Name?.Trim()))
      {
        builder.AddParam("name", options.Name.Trim());
      }

      if (!string.IsNullOrEmpty(options.Code?.Trim()))
      {
        builder.AddParam("code", options.Code.Trim());
      }

      if (!string.IsNullOrEmpty(options.GroupCode?.Trim()))
      {
        builder.AddParam("groupCode", options.GroupCode.Trim());
      }

      if (!string.IsNullOrEmpty(options.CertificateNo?.Trim()))
      {
        builder.AddParam("certificateNo", options.CertificateNo.Trim());
      }

      if (!string.IsNullOrEmpty(options.Premise?.Trim()))
      {
        builder.AddParam("premise", options.Premise.Trim());
      }

      if (options.PremiseID > 0L)
      {
        builder.AddParam("premiseID", $"{options.PremiseID}");
      }

      if (options.Status?.Any() ?? false)
      {
        for (int i = 0; i < options.Status.Count; i++)
        {
          builder.AddParam($"status[{i}]", $"{(int)options.Status[i]}");
        }
      }

      return builder.Execute<IList<Certificate360>>();
    }

    public Task<IList<Request>> GetCustomerRecentRequest(Guid id)
    {
      return _requestProvider.BuildUpon(_requestUrl)
        .Uri($"/api/customer/{id}/request/recent")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Request>>();
    }

    public async Task<IList<Customer>> List()
    {
      return await _requestProvider.BuildUpon(_requestUrl)
        .Uri($"/api/customer/list")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Customer>>();
    }

    public Task<Customer> SetParent(Guid id, Guid parentID)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/customer/{id}/parent/{parentID}")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Customer>();
    }
  }
}
