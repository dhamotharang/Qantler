using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class BillSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public BillSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<Bill>> List(BillFilter filter)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/bill/list")
        .AddParam("id", filter.ID != null ? $"{filter.ID.Value}" : null)
        .AddParam("requestID", filter.RequestID != null ? $"{filter.RequestID.Value}" : null)
        .AddParam("refNo", filter.RefNo)
        .AddParam("invoiceNo", filter.InvoiceNo)
        .AddParam("refID", filter.RefID)
        .AddParam("customerName", filter.CustomerName)
        .AddParam("status", filter.Status != null ? $"{filter.Status}" : null)
        .AddParam("from", filter.From?.UtcDateTime.ToString())
        .AddParam("to", filter.To?.UtcDateTime.ToString())
        .AddParam("type", filter.Type != null ? $"{filter.Type}" : null)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Bill>>();
    }

    public async Task AddLineItems(long billID, IList<BillLineItem> lineItems)
    {
      await _requestProvider.BuildUpon(_url)
        .Uri($"/api/bill/{billID}/lineItem")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(lineItems))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<Bill> GetByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/Bill/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Bill>();
    }
  }
}
