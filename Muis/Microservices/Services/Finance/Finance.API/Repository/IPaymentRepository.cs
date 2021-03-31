using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Finance.API.Services;
using Finance.Model;

namespace Finance.API.Repository
{
  public interface IPaymentRepository
  {
    /// <summary>
    /// Insert payment.
    /// </summary>
    Task<long> Insert(Payment payment);

    /// <summary>
    /// Query payments based on specified options.
    /// </summary>
    Task<IList<Payment>> Query(PaymentFilter options);

    /// <summary>
    /// Get recent payment for specified customer
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="rowFrom">record start from</param>
    /// <param name="rowCount">record count</param>
    /// <returns></returns>
    Task<IList<Payment>> GetCustomerRecentPayment(Guid ID,
      long? rowFrom = 0,
      long? rowCount = 10);

    /// <summary>
    /// Retrieve payment info based on specified ID.
    /// </summary>
    Task<Payment> GetPaymentByID(long id);

    /// <summary>
    /// Exec payment action.
    /// </summary>
    Task ExecPaymentAction(long id, PaymentStatus status, Officer officer);

    /// <summary>
    /// map payment to note
    /// </summary>
    Task MapNote(long paymentId, long noteId);

    /// <summary>
    /// map payment to log
    /// </summary>
    Task MapLog(long paymentId, long logId);

    /// <summary>
    /// Map payment bill.
    /// </summary>
    Task MapBill(long paymentID, long billID);

    /// <summary>
    /// Map payment bank.
    /// </summary>
    Task MapBank(long paymentID, long bankID);

    /// <summary>
    /// delete map payment bank
    /// </summary>
    public Task DeleteMapBank(long paymentID, long bankID);

    /// <summary>
    /// Get bank details from map payment bank 
    /// </summary>
    public Task<long?> GetMapBank(long paymentID);
  }
}
