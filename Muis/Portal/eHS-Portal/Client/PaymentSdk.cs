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
  public class PaymentSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public PaymentSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<Payment> GetPaymentByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/payment/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Payment>();
    }

    public Task<IList<Payment>> GetCustomerRecentPayment(Guid id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/payment/{id}/recent")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Payment>>();
    }

    public Task<IList<Payment>> List(PaymentFilter filter)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/payment/list")
        .AddParam("id", filter.ID != null ? $"{filter.ID}" : null)
        .AddParam("customerId", filter.CustomerID != null ? $"{filter.CustomerID}" : null)
        .AddParam("name", string.IsNullOrEmpty(filter.Name?.Trim())
          ? null
          : filter.Name.Trim())
        .AddParam("transactionNo", string.IsNullOrEmpty(filter.TransactionNo?.Trim())
          ? null
          : filter.TransactionNo.Trim())
        .AddParam("status", filter.Status != null ? $"{filter.Status}" : null)
        .AddParam("method", filter.Method != null ? $"{filter.Method}" : null)
        .AddParam("mode", filter.Mode != null ? $"{filter.Mode}" : null)
        .AddParam("from", filter.From?.UtcDateTime.ToString())
        .AddParam("to", filter.To?.UtcDateTime.ToString())
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Payment>>();
    }

    public Task<Payment> PaymentAction(long id, PaymentStatus status, Bank bank)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/payment/{id}/action")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(bank))
        .AddParam("status", $"{(int)status}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Payment>();
    }

    public Task<Notes> AddNote(long id, Notes note)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/payment/{id}/note")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(note))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Notes>();
    }

    public Task<Payment> PaymentForComposition(PaymentForComposition payment)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/payment/composition")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(payment))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Payment>();
    }
  }
}
