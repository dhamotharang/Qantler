using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Payment
{
  public interface IPaymentService
  {
    /// <summary>
    /// Retrieve payments based on specified filter.
    /// </summary>
    Task<IList<Model.Payment>> List(PaymentFilter filter);

    /// <summary>
    /// Retrieve payment instance with specified ID.
    /// </summary>
    Task<Model.Payment> GetPaymentByID(long id);

    /// <summary>
    /// Get recent payment for specified customer
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IList<Model.Payment>> GetCustomerRecentPayment(Guid id);

    /// <summary>
    /// Payment action.
    /// </summary>
    Task<Model.Payment> PaymentAction(long id, PaymentStatus status, Model.Bank bank);

    /// <summary>
    /// save note
    /// </summary>
    Task<Notes> AddNote(long id, Notes note);
  }
}
