using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Finance.API.DTO;
using Finance.Model;

namespace Finance.API.Services
{
  public interface IPaymentService
  {
    /// <summary>
    /// Filter payments based on specified options.
    /// </summary>
    Task<IList<Payment>> Filter(PaymentFilter options);

    /// <summary>
    /// Retrieve payment instance with specified ID.
    /// </summary>
    Task<Payment> GetPaymentByID(long id);

    /// <summary>
    /// Get recent payment for specified customer
    /// </summary>
    /// <param name="customerID"></param>
    /// <returns></returns>
    Task<IList<Payment>> GetCustomerRecentPayment(Guid customerID);

    /// <summary>
    /// Trigger payment action.
    /// </summary>
    Task<Payment> PaymentAction(long id, PaymentStatus status, Bank bank, Officer officer);

    /// <summary>
    /// Save note
    /// </summary>
    Task<Note> AddNote(long id, Note note, Officer user);

    /// <summary>
    /// Composition sum payment.
    /// </summary>
    Task<Payment> PaymentForComposition(PaymentForComposition param);
  }

  public class PaymentFilter
  {
    public long? ID { get; set; }

    public Guid? CustomerID { get; set; }

    public string Name { get; set; }

    public string TransactionNo { get; set; }

    public PaymentStatus? Status { get; set; }

    public PaymentMode? Mode { get; set; }

    public PaymentMethod? Method { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }
  }
}
