using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.API.Strategies.Billing;
using Finance.Model;

namespace Finance.API.Services
{
  public interface IBillService
  {
    /// <summary>
    /// Filter bill based on specified parameters.
    /// </summary>
    Task<IList<Bill>> Filter(BillFilter filter);

    /// <summary>
    /// Request to generate bill on specified request.
    /// </summary>
    Task<Bill> GenerateBill(BillRequest request);

    /// <summary>
    /// Update specified bill.
    /// </summary>
    Task UpdateBill(Bill bill);

    /// <summary>
    /// Add line item to bill.
    /// </summary>
    Task AddLineItem(long id, IList<BillLineItem> lineItems);

    /// <summary>
    /// Get bill by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Bill> GetBillByID(long id);
  }

  public class BillFilter
  {
    public long? ID { get; set; }

    public string InvoiceNo { get; set; }

    public long? RequestID { get; set; }

    public string RefNo { get; set; }

    public string CustomerName { get; set; }

    public string RefID { get; set; }

    public BillStatus? Status { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public BillType? Type { get; set; }
  }
}
