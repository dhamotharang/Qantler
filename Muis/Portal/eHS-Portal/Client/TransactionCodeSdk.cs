using Core.Http;
using eHS.Portal.DTO;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class TransactionCodeSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public TransactionCodeSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<TransactionCode>> Query(TransactionCode options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/TransactionCode/query")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (!string.IsNullOrEmpty(options.Code?.Trim()))
      {
        builder.AddParam("Code", $"{options.Code}");
      }

      if (!string.IsNullOrEmpty(options.GLEntry?.Trim()))
      {
        builder.AddParam("GLEntry", options.GLEntry.Trim());
      }

      if (!string.IsNullOrEmpty(options.Text?.Trim()))
      {
        builder.AddParam("Text", options.Text.Trim());
      }

      return builder.Execute<IList<TransactionCode>>();
    }

    public Task<TransactionCodeDTO> GetByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/TransactionCode/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<TransactionCodeDTO>();
    }

    public Task<bool> UpdatePrice(List<Price> priceDetails)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/TransactionCode")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(priceDetails))
        .Execute<bool>();
    }
  }
}