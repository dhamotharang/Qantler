using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Payment
{
  public class PaymentService : IPaymentService
  {
    readonly ApiClient _apiClient;

    public PaymentService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Model.Payment> GetPaymentByID(long id)
    {
      return await _apiClient.PaymentSdk.GetPaymentByID(id);
    }

    public async Task<IList<Model.Payment>> List(PaymentFilter filter)
    {
      return await _apiClient.PaymentSdk.List(filter);
    }

    public async Task<Model.Payment> PaymentAction(long id, PaymentStatus status, Model.Bank bank)
    {
      return await _apiClient.PaymentSdk.PaymentAction(id, status, bank);
    }

    public async Task<Notes> AddNote(long id, Notes note)
    {
      return await _apiClient.PaymentSdk.AddNote(id, note);
    }

    public async Task<IList<Model.Payment>> GetCustomerRecentPayment(Guid id)
    {
      return await _apiClient.PaymentSdk.GetCustomerRecentPayment(id);
    }
  }
}
